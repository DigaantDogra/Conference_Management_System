namespace ConferenceManagement.Models
{
    public class HomeViewModel
    {
        public List<Event> FeaturedEvents { get; set; } = new List<Event>();
        public List<Speaker> FeaturedSpeakers { get; set; } = new List<Speaker>();
        public List<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
    }

    public class Speaker
    {
        public string Name { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
    }

    public class Testimonial
    {
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}