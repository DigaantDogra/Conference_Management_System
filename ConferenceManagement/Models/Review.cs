using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceManagement.Models
{
    public class Review
    {
        public int Id { get; set; } // Unique identifier for the review

        [Required]
        public int EventId { get; set; } // The ID of the event being reviewed

        [Required]
        public string UserName { get; set; } // The name of the user submitting the review

        [Required]
        [StringLength(500, ErrorMessage = "The review content must be between 10 and 500 characters.", MinimumLength = 10)]
        public string Content { get; set; } // The review text

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // Rating out of 5

        public DateTime DatePosted { get; set; } = DateTime.Now; // Timestamp for when the review was submitted

        // Navigation property to associate the review with an event
        public Event Event { get; set; }
    }
}
