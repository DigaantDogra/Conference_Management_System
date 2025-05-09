﻿namespace ConferenceManagement.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        // Helper property for full address
        public string FullAddress => $"{Address}, {City}, {Country} {PostalCode}".Trim();
    }
}