using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceManagement.Models
{
    public class Booking
    {
        public TicketType TicketT { get; set; }
        public int Capacity { get; set; }
        public required Event EventBook { get; set; }

        public int BookingId { get; set; }

    }
}