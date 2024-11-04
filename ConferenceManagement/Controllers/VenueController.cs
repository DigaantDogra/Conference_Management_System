using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using ConferenceManagement.Services;
using System.Linq;

namespace ConferenceManagement.Controllers
{
    public class VenueController : Controller
    {
        public IActionResult Index()
        {
            return View(DummyDataService.Venues);
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
                venue.Id = DummyDataService.Venues.Max(v => v.Id) + 1;
                DummyDataService.Venues.Add(venue);
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public IActionResult Details(int id)
        {
            var venue = DummyDataService.Venues.FirstOrDefault(v => v.Id == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
    }
}