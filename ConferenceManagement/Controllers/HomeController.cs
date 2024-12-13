using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Config;
using ConferenceManagement.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConferenceManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();

                // Fetch Featured Events
                MySqlCommand eventCommand = new MySqlCommand(
                    "SELECT Id, Title, Description, DateFrom, DateTo, ConferenceType, VenueName FROM Events",
                    connection);

                using (var reader = eventCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        viewModel.FeaturedEvents.Add(new Event
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateFrom = reader.GetDateTime(3),
                            DateTo = reader.GetDateTime(4),
                            ConferenceType = Enum.Parse<ConferenceType>(reader.GetString(5)),
                            VenueName = reader.GetString(6)
                        });
                    }
                }

                // Fetch Featured Speakers
                MySqlCommand speakerCommand = new MySqlCommand(
                    "SELECT Name, EventName FROM Speakers",
                    connection);

                using (var reader = speakerCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        viewModel.FeaturedSpeakers.Add(new Speaker
                        {
                            Name = reader.GetString(0),
                            EventName = reader.GetString(1)
                        });
                    }
                }
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}