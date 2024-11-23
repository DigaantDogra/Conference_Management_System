using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Models;
using System;
using System.Collections.Generic;

namespace ConferenceManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Create dummy data
            var viewModel = new HomeViewModel
            {
                FeaturedEvents = new List<Event>
                {
                    new Event
                    {
                        Id = 1,
                        Title = "Tech Innovation Summit 2024",
                        Description = "Annual technology conference showcasing latest innovations",
                        DateFrom = DateTime.Now.AddMonths(1),
                        DateTo = DateTime.Now.AddMonths(1).AddDays(3),
                        ConferenceType = ConferenceType.Hybrid,
                        VenueName = "City Center Convention Hall"
                    },
                    new Event
                    {
                        Id = 2,
                        Title = "Digital Marketing Workshop",
                        Description = "Intensive workshop on digital marketing strategies",
                        DateFrom = DateTime.Now.AddMonths(2),
                        DateTo = DateTime.Now.AddMonths(2).AddDays(1),
                        ConferenceType = ConferenceType.InPerson,
                        VenueName = "Innovation Hub"
                    },
                    new Event
                    {
                        Id = 3,
                        Title = "Virtual Developer Conference",
                        Description = "Online conference for software developers",
                        DateFrom = DateTime.Now.AddMonths(3),
                        DateTo = DateTime.Now.AddMonths(3).AddDays(2),
                        ConferenceType = ConferenceType.Online,
                        VenueName = "Virtual Platform"
                    }
                },
                FeaturedSpeakers = new List<Speaker>
                {
                    new Speaker { Name = "John Doe", EventName = "Tech Summit 2024" },
                    new Speaker { Name = "Jane Smith", EventName = "Digital Marketing Workshop" },
                    new Speaker { Name = "Mike Johnson", EventName = "AI Conference" },
                    new Speaker { Name = "Sarah Wilson", EventName = "Web Development Summit" }
                },
                Testimonials = new List<Testimonial>
                {
                    new Testimonial
                    {
                        Text = "An amazing conference that exceeded all expectations!",
                        Author = "John Smith",
                        Role = "CEO, Tech Corp"
                    },
                    new Testimonial
                    {
                        Text = "The best tech conference I've attended this year.",
                        Author = "Jane Doe",
                        Role = "Software Engineer"
                    }
                }
            };

            return View(viewModel);
        }

        // public IActionResult Login()=>View();

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}