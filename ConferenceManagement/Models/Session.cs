using Microsoft.Extensions.Logging;
using System;

namespace ConferenceManagement.Models
{
    public enum SessionType
    {
        Presentation,
        Workshop,
        RoundTable,
        Panels,
        SportsEvent
    }

    public class Session
    {
        public int Id { get; set; }
        public SessionType SessionType { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsOnline { get; set; }
        public double Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string SessionTitle { get; set; }
        public string? Description { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }

}
