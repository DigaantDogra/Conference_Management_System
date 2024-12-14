using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceManagement.Services
{
    public static class DummyDataService
    {
        // First, initialize Events as it's used in Bookings
        public static List<Event> Events { get; } = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Tech Innovation Summit 2024",
                Description = "Annual technology conference showcasing latest innovations",
                DateFrom = DateTime.Now.AddMonths(1),
                DateTo = DateTime.Now.AddMonths(1).AddDays(3),
                ConferenceType = ConferenceType.Hybrid,
                VenueId = 1,
                AttendeeLimit = 500,
                Fee = 299.99,
                
            },
            new Event
            {
                Id = 2,
                Title = "Digital Marketing Workshop",
                Description = "Intensive workshop on digital marketing strategies",
                DateFrom = DateTime.Now.AddMonths(2),
                DateTo = DateTime.Now.AddMonths(2).AddDays(1),
                ConferenceType = ConferenceType.InPerson,
                VenueId = 2,
                AttendeeLimit = 100,
                Fee = 149.99,
                
            },
            new Event
            {
                Id = 3,
                Title = "Virtual Developer Conference",
                Description = "Online conference for software developers",
                DateFrom = DateTime.Now.AddMonths(3),
                DateTo = DateTime.Now.AddMonths(3).AddDays(2),
                ConferenceType = ConferenceType.Online,
                VenueId = 3,
                AttendeeLimit = 1000,
                Fee = 99.99,
                
            }
        };

        // Then initialize Bookings using Events
        public static List<Booking> Bookings { get; } = new List<Booking>
        {
            new Booking
            {
                BookingId = 1,
                TicketT = TicketType.Regular,
                Capacity = 1,
                EventBook = Events[0] // Using index instead of First()
            },
            new Booking
            {
                BookingId = 2,
                TicketT = TicketType.VIP,
                Capacity = 2,
                EventBook = Events[1] // Using index instead of Skip(1).First()
            }
        };

        public static Booking GetBookingById(int id)
        {
            return Bookings.FirstOrDefault(b => b.BookingId == id);
        }

        public static List<Venue> Venues { get; } = new List<Venue>
        {
            new Venue
            {
                Id = 1,
                Address = "123 Main Street, City Center",
                Capacity = 500,
                Facilities = "WiFi, Projector, Audio System",
                Layout = Layout.Large,
                IsOnline = false
            },
            new Venue
            {
                Id = 2,
                Address = "456 Tech Avenue, Innovation Hub",
                Capacity = 200,
                Facilities = "Video Conferencing, Interactive Displays",
                Layout = Layout.Medium,
                IsOnline = false
            },
            new Venue
            {
                Id = 3,
                Address = "Virtual Conference Platform",
                Capacity = 1000,
                Facilities = "Virtual Breakout Rooms, Live Streaming",
                Layout = Layout.Large,
                IsOnline = true
            }
        };

        public static List<Session> Sessions { get; } = new List<Session>
        {
            new Session
            {
                Id = 1,
                SessionType = SessionType.Presentation,
                IsExclusive = true,
                IsOnline = false,
                Duration = 2.0,
                StartTime = DateTime.Now.AddMonths(1).AddHours(9),
                SessionTitle = "Keynote: Future of Technology",
                Description = "Opening keynote discussing future tech trends",
                EventId = 1
            },
            new Session
            {
                Id = 2,
                SessionType = SessionType.Workshop,
                IsExclusive = false,
                IsOnline = false,
                Duration = 3.0,
                StartTime = DateTime.Now.AddMonths(1).AddHours(13),
                SessionTitle = "Hands-on AI Workshop",
                Description = "Interactive workshop on implementing AI solutions",
                EventId = 1
            },
            new Session
            {
                Id = 3,
                SessionType = SessionType.RoundTable,
                IsExclusive = false,
                IsOnline = true,
                Duration = 1.5,
                StartTime = DateTime.Now.AddMonths(1).AddHours(16),
                SessionTitle = "Industry Expert Panel",
                Description = "Discussion with leading industry experts",
                EventId = 1
            }
        };

public static List<Review> Reviews = new List<Review>
{
            new Review
            {
                Id = 1,
                EventId = 1,
                UserName = "John Doe",
                Content = "This event was amazing! Highly recommended.",
                Rating = 5,
                DatePosted = DateTime.Now.AddDays(-3)
            },
            new Review
            {
                Id = 2,
                EventId = 1,
                UserName = "Jane Smith",
                Content = "Good event but could be better organized.",
                Rating = 3,
                DatePosted = DateTime.Now.AddDays(-1)
            }
        };

    }
}