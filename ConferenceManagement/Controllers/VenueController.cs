using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConferenceManagement.Config;
using ConferenceManagement.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ConferenceManagement.Controllers
{
    public class VenueController : Controller
    {
        public IActionResult Index()
        {
            var venues = new List<Venue>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT Id, Address, Capacity, Facilities, Layout, IsOnline FROM Venues", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var layoutString = reader.GetString("Layout");
                            Layout layout;
                            if (Enum.TryParse<Layout>(layoutString, out layout))
                            {
                                venues.Add(new Venue
                                {
                                    Id = reader.GetInt32("Id"),
                                    Address = reader.GetString("Address"),
                                    Capacity = reader.GetInt32("Capacity"),
                                    Facilities = reader.GetString("Facilities"),
                                    Layout = layout,
                                    IsOnline = reader.GetBoolean("IsOnline")
                                });
                            }
                        }
                    }
                }
                return View(venues);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading venues: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(
                            "INSERT INTO Venues (Address, Capacity, Facilities, Layout, IsOnline) " +
                            "VALUES (@Address, @Capacity, @Facilities, @Layout, @IsOnline)",
                            connection);

                        command.Parameters.Add("@Address", MySqlDbType.VarChar).Value = venue.Address;
                        command.Parameters.Add("@Capacity", MySqlDbType.Int32).Value = venue.Capacity;
                        command.Parameters.Add("@Facilities", MySqlDbType.Text).Value =
                            string.IsNullOrEmpty(venue.Facilities) ? DBNull.Value : (object)venue.Facilities;
                        command.Parameters.Add("@Layout", MySqlDbType.Enum).Value = venue.Layout.ToString();
                        command.Parameters.Add("@IsOnline", MySqlDbType.Bit).Value = venue.IsOnline;

                        command.ExecuteNonQuery();
                    }
                    TempData["SuccessMessage"] = "Venue created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating venue: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while creating the venue.");
                }
            }
            return View(venue);
        }

        public IActionResult Details(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(
                        "SELECT Id, Address, Capacity, Facilities, Layout, IsOnline FROM Venues WHERE Id = @Id",
                        connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var layoutString = reader.GetString("Layout");
                            Layout layout;
                            if (Enum.TryParse<Layout>(layoutString, out layout))
                            {
                                var venue = new Venue
                                {
                                    Id = reader.GetInt32("Id"),
                                    Address = reader.GetString("Address"),
                                    Capacity = reader.GetInt32("Capacity"),
                                    Facilities = reader.GetString("Facilities"),
                                    Layout = layout,
                                    IsOnline = reader.GetBoolean("IsOnline")
                                };
                                return View(venue);
                            }
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading venue details: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}