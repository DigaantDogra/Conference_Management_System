using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using ConferenceManagement.Services;
using System.Linq;

namespace ConferenceManagement.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review/Create
        public IActionResult Create(int eventId)
        {
            ViewBag.Event = DummyDataService.Events.FirstOrDefault(e => e.Id == eventId); // Load event details
            return View(new Review { EventId = eventId });
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Id = DummyDataService.Reviews.Any()
                    ? DummyDataService.Reviews.Max(r => r.Id) + 1
                    : 1;

                DummyDataService.Reviews.Add(review);
                TempData["SuccessMessage"] = "Thank you for your review!";
                return RedirectToAction("Details", "Event", new { id = review.EventId });
            }

            ViewBag.Event = DummyDataService.Events.FirstOrDefault(e => e.Id == review.EventId);
            return View(review);
        }

        // GET: Review/List
        public IActionResult List(int eventId)
        {
            var reviews = DummyDataService.Reviews.Where(r => r.EventId == eventId).ToList();
            ViewBag.Event = DummyDataService.Events.FirstOrDefault(e => e.Id == eventId);
            return View(reviews);
        }
    }
}
