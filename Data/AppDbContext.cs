using Microsoft.EntityFrameworkCore;
using WebQuiz.Models;

namespace WebQuiz.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Result> results { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Ruler> rulers { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<Role> roles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
