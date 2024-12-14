using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(100, ErrorMessage = "The title must be between 3 and 100 characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The description must be between 10 and 500 characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Compare(nameof(DateFrom), ErrorMessage = "The end date must be after the start date.")]
        public DateTime DateTo { get; set; }

        [Required]
        public ConferenceType ConferenceType { get; set; }

        [Required]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [Range(1, 10000, ErrorMessage = "Attendee limit must be between 1 and 10,000.")]
        public int AttendeeLimit { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "The fee must be between $0.00 and $10,000.00.")]
        public double Fee { get; set; }

        public List<Session> Sessions { get; set; }

    }
}
