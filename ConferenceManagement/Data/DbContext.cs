using ConferenceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
