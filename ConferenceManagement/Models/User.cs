using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceManagement.Models
{
    public class User
    {
        public Roles UserType { get; set; }
        public required string Email { get; set; }
        public required Booking Bookings { get; set; }
        public SponsorshipLevel Sponsor { get; set; }

        public void AddBookings(){ // parameter is Booking type

        }// not needed after data prisistance

        public void DeleteBookings(){ // parameter is Booking type

        }// not needed after data prisistance
    }
}