using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using ConferenceManagement.Services;

namespace ConferenceManagement.Controllers
{
    public class EventController : Controller
    {
        // GET: Event/Create
        public IActionResult Create()
        {
            try
            {
                // Load venue data for dropdown
                ViewBag.Venues = DummyDataService.Venues;

                // Return the Create view
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception (logging can be implemented)
                Console.WriteLine($"Error in Create (GET): {ex.Message}");

                // Redirect to an error page or show an error message
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
                // Validation failed, reload venue data and return the view with errors
                ViewBag.Venues = DummyDataService.Venues;
                return View(newEvent);
            }

            try
            {
                // Generate a new unique ID for the event
                newEvent.Id = DummyDataService.Events.Any()
                    ? DummyDataService.Events.Max(e => e.Id) + 1
                    : 1;

                // Add the new event to the dummy data service
                DummyDataService.Events.Add(newEvent);

                // Redirect to the home page or event listing after successful creation
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception (logging can be implemented)
                Console.WriteLine($"Error in Create (POST): {ex.Message}");

                // Show an error message and reload the view
                ViewBag.ErrorMessage = "An error occurred while creating the event. Please try again.";
                ViewBag.Venues = DummyDataService.Venues;
                return View(newEvent);
            }
        }
    }
}
