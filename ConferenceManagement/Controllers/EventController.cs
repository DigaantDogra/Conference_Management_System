using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using ConferenceManagement.Data;
using System.Linq;

namespace ConferenceManagement.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var venues = _context.Venues.ToList();
            ViewBag.Venues = venues;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event newEvent, Venue newVenue)
        {
            /*if (!ModelState.IsValid)
            {
                ViewBag.Venues = _context.Venues.ToList();
                return View(newEvent);
            }*/

            try
            {
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error saving event: {ex.Message}";
                ViewBag.Venues = _context.Venues.ToList();
                return View(newEvent);
            }
        }
    }
}
