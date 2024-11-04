using System;
using System.Collections.Generic;

namespace ConferenceManagement.Models
{
    public enum ConferenceType
    {
        InPerson,
        Online,
        Hybrid
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ConferenceType ConferenceType { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public int AttendeeLimit { get; set; }
        public double Fee { get; set; }
        public List<Session> Sessions { get; set; }
        public string VenueName { get; set; }
    }
}