using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ConferenceManagement.Config;
using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceManagement.Controllers
{
    public class EventController : Controller
    {
        // GET: Event/Index
        public IActionResult Index()
        {
            var events = new List<Event>();
            try
            {
                using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT e.*, v.Address as VenueAddress 
                        FROM Events e 
                        LEFT JOIN Venues v ON e.VenueId = v.Id", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(new Event
                                {
                                    Id = reader.GetInt32("Id"),
                                    Title = reader.GetString("Title"),
                                    Description = reader.GetString("Description"),
                                    DateFrom = reader.GetDateTime("DateFrom"),
                                    DateTo = reader.GetDateTime("DateTo"),
                                    ConferenceType = Enum.Parse<ConferenceType>(reader.GetString("ConferenceType")),
                                    VenueId = reader.GetInt32("VenueId"),
                                    VenueName = reader.GetString("VenueName"),
                                    AttendeeLimit = reader.GetInt32("AttendeeLimit"),
                                    Fee = reader.GetDouble("Fee")
                                });
                            }
                        }
                    }
                }
                return View(events);
            }
            catch (Exception ex)
            {
                // Log the error
                TempData["ErrorMessage"] = "An error occurred while loading events.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            try
            {
                LoadDropDowns();
                return View(new Event { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while preparing the create form.";
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(newEvent);
            }

            try
            {
                using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();

                    // Get venue address using transaction
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string venueAddress = "";
                            using (var command = new MySqlCommand("SELECT Address FROM Venues WHERE Id = @VenueId", connection, transaction))
                            {
                                command.Parameters.AddWithValue("@VenueId", newEvent.VenueId);
                                var result = command.ExecuteScalar();
                                venueAddress = result?.ToString() ?? "Unknown Venue";
                            }

                            using (var command = new MySqlCommand(@"
                                INSERT INTO Events (Title, Description, DateFrom, DateTo, 
                                    ConferenceType, VenueId, VenueName, AttendeeLimit, Fee)
                                VALUES (@Title, @Description, @DateFrom, @DateTo, 
                                    @ConferenceType, @VenueId, @VenueName, @AttendeeLimit, @Fee)",
                                connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Title", newEvent.Title);
                                command.Parameters.AddWithValue("@Description", newEvent.Description);
                                command.Parameters.AddWithValue("@DateFrom", newEvent.DateFrom);
                                command.Parameters.AddWithValue("@DateTo", newEvent.DateTo);
                                command.Parameters.AddWithValue("@ConferenceType", newEvent.ConferenceType.ToString());
                                command.Parameters.AddWithValue("@VenueId", newEvent.VenueId);
                                command.Parameters.AddWithValue("@VenueName", venueAddress);
                                command.Parameters.AddWithValue("@AttendeeLimit", newEvent.AttendeeLimit);
                                command.Parameters.AddWithValue("@Fee", newEvent.Fee);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the event.");
                LoadDropDowns();
                return View(newEvent);
            }
        }

        // GET: Event/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT e.*, v.Address as VenueAddress, v.Capacity as VenueCapacity,
                               v.Facilities, v.Layout, v.IsOnline
                        FROM Events e 
                        LEFT JOIN Venues v ON e.VenueId = v.Id 
                        WHERE e.Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var eventDetails = new Event
                                {
                                    Id = reader.GetInt32("Id"),
                                    Title = reader.GetString("Title"),
                                    Description = reader.GetString("Description"),
                                    DateFrom = reader.GetDateTime("DateFrom"),
                                    DateTo = reader.GetDateTime("DateTo"),
                                    ConferenceType = Enum.Parse<ConferenceType>(reader.GetString("ConferenceType")),
                                    VenueId = reader.GetInt32("VenueId"),
                                    VenueName = reader.GetString("VenueName"),
                                    AttendeeLimit = reader.GetInt32("AttendeeLimit"),
                                    Fee = reader.GetDouble("Fee"),
                                    Venue = new Venue
                                    {
                                        Id = reader.GetInt32("VenueId"),
                                        Address = reader.GetString("VenueAddress"),
                                        Capacity = reader.GetInt32("VenueCapacity"),
                                        Facilities = reader.GetString("Facilities"),
                                        Layout = Enum.Parse<Layout>(reader.GetString("Layout")),
                                        IsOnline = reader.GetBoolean("IsOnline")
                                    }
                                };
                                return View(eventDetails);
                            }
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the event details.";
                return RedirectToAction("Error", "Home");
            }
        }

        private void LoadDropDowns()
        {
            // Load venues for dropdown
            var venues = new List<Venue>();
            using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT Id, Address FROM Venues", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            venues.Add(new Venue
                            {
                                Id = reader.GetInt32("Id"),
                                Address = reader.GetString("Address")
                            });
                        }
                    }
                }
            }

            // Convert to SelectListItems
            ViewBag.Venues = venues.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Address
            });

            // Add ConferenceType options
            ViewBag.ConferenceTypes = Enum.GetValues(typeof(ConferenceType))
                .Cast<ConferenceType>()
                .Select(ct => new SelectListItem
                {
                    Value = ct.ToString(),
                    Text = ct.ToString()
                });
        }
    }
}