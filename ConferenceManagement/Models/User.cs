using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceManagement.Models
{
    public class User
    {
        [Required]
        public Roles UserType { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
        public Booking? Bookings { get; set; }
        public SponsorshipLevel Sponsor { get; set; }

        public void AddBookings(){ // parameter is Booking type

        }// not needed after data prisistance

        public void DeleteBookings(){ // parameter is Booking type

        }// not needed after data prisistance

        // testing purpose section -----------------
        [Required]
        public string Username { get; set; }
        [Required]
        public string HashPass { get; set; } 
    }
}