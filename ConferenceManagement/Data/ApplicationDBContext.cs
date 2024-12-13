using Microsoft.EntityFrameworkCore;
using ConferenceManagement.Models;

namespace ConferenceManagement.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // Define DbSets for all models
        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
    }
}
