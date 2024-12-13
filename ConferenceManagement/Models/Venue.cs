namespace ConferenceManagement.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string Facilities { get; set; }
        public Layout Layout { get; set; }
        public bool IsOnline { get; set; }
    }

    public enum Layout
    {
        Small,
        Medium,
        Large
    }
}