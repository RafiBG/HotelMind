using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelMind.Models;

namespace HotelMind.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tells SQL to create a table for Registration data
        public DbSet<RegisterDBModel> RegisteredUsers { get; set; }

    }
}