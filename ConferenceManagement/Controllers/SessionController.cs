using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using ConferenceManagement.Services;
using System.Linq;

namespace ConferenceManagement.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View(DummyDataService.Sessions);
        }

        public IActionResult Create()
        {
            ViewBag.Events = DummyDataService.Events;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Session session)
        {
            if (ModelState.IsValid)
            {
                session.Id = DummyDataService.Sessions.Max(s => s.Id) + 1;
                DummyDataService.Sessions.Add(session);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = DummyDataService.Events;
            return View(session);
        }

        public IActionResult Details(int id)
        {
            var session = DummyDataService.Sessions.FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }
    }
}
