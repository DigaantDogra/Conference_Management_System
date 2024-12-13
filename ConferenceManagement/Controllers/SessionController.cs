using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConferenceManagement.Config;
using ConferenceManagement.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ConferenceManagement.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Create()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT Id, Title FROM Events", connection);
                    var events = new List<SelectListItem>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new SelectListItem
                            {
                                Value = reader.GetInt32("Id").ToString(),
                                Text = reader.GetString("Title")
                            });
                        }
                    }
                    ViewBag.Events = new SelectList(events, "Value", "Text");
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Session session)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoadEventsList();
                    return View(session);
                }

                using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            INSERT INTO Sessions (
                                SessionType, 
                                IsExclusive, 
                                IsOnline, 
                                Duration, 
                                StartTime, 
                                SessionTitle, 
                                Description, 
                                EventId
                            ) VALUES (
                                @SessionType, 
                                @IsExclusive, 
                                @IsOnline, 
                                @Duration, 
                                @StartTime, 
                                @SessionTitle, 
                                @Description, 
                                @EventId
                            )";

                        // Add parameters with explicit types
                        command.Parameters.Add("@SessionType", MySqlDbType.Enum).Value = session.SessionType.ToString();
                        command.Parameters.Add("@IsExclusive", MySqlDbType.Bit).Value = session.IsExclusive;
                        command.Parameters.Add("@IsOnline", MySqlDbType.Bit).Value = session.IsOnline;
                        command.Parameters.Add("@Duration", MySqlDbType.Double).Value = session.Duration;
                        command.Parameters.Add("@StartTime", MySqlDbType.DateTime).Value = session.StartTime;
                        command.Parameters.Add("@SessionTitle", MySqlDbType.VarChar).Value = session.SessionTitle;
                        command.Parameters.Add("@Description", MySqlDbType.Text).Value =
                            string.IsNullOrEmpty(session.Description) ? DBNull.Value : (object)session.Description;
                        command.Parameters.Add("@EventId", MySqlDbType.Int32).Value = session.EventId;

                        // Execute the command
                        command.ExecuteNonQuery();

                        // Log the SQL and parameters for debugging
                        Console.WriteLine("SQL: " + command.CommandText);
                        foreach (MySqlParameter param in command.Parameters)
                        {
                            Console.WriteLine($"Parameter {param.ParameterName}: {param.Value}");
                        }
                    }
                }

                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error creating session: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "An error occurred while creating the session. Please try again.");
                LoadEventsList();
                return View(session);
            }
        }

        private void LoadEventsList()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT Id, Title FROM Events", connection);
                var events = new List<SelectListItem>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new SelectListItem
                        {
                            Value = reader.GetInt32("Id").ToString(),
                            Text = reader.GetString("Title")
                        });
                    }
                }
                ViewBag.Events = new SelectList(events, "Value", "Text");
            }
        }

        public IActionResult Index()
        {
            var sessions = new List<Session>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();
                var command = new MySqlCommand(@"
                    SELECT s.*, e.Title as EventTitle 
                    FROM Sessions s 
                    LEFT JOIN Events e ON s.EventId = e.Id", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new Session
                        {
                            Id = reader.GetInt32("Id"),
                            SessionTitle = reader.GetString("SessionTitle"),
                            Description = reader.GetString("Description"),
                            SessionType = Enum.Parse<SessionType>(reader.GetString("SessionType")),
                            IsExclusive = reader.GetBoolean("IsExclusive"),
                            IsOnline = reader.GetBoolean("IsOnline"),
                            Duration = reader.GetDouble("Duration"),
                            StartTime = reader.GetDateTime("StartTime"),
                            EventId = reader.GetInt32("EventId")
                        });
                    }
                }
            }
            return View(sessions);
        }

        public IActionResult Details(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    // Get session details
                    var command = new MySqlCommand(@"
                SELECT s.*, e.Title as EventTitle 
                FROM Sessions s 
                LEFT JOIN Events e ON s.EventId = e.Id 
                WHERE s.Id = @Id", connection);

                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var session = new Session
                            {
                                Id = reader.GetInt32("Id"),
                                SessionTitle = reader.GetString("SessionTitle"),
                                Description = reader.GetString("Description"),
                                SessionType = Enum.Parse<SessionType>(reader.GetString("SessionType")),
                                IsExclusive = reader.GetBoolean("IsExclusive"),
                                IsOnline = reader.GetBoolean("IsOnline"),
                                Duration = reader.GetDouble("Duration"),
                                StartTime = reader.GetDateTime("StartTime"),
                                EventId = reader.GetInt32("EventId")
                            };

                            // Store the event title in ViewBag
                            ViewBag.EventTitle = reader.GetString("EventTitle");

                            return View(session);
                        }
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}