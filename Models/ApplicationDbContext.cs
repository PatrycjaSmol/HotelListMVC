using Microsoft.EntityFrameworkCore;

namespace HotelListMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
