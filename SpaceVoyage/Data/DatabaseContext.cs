using Microsoft.EntityFrameworkCore;

namespace SpaceVoyage.Data
{
    public class DatabaseContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DatabaseContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("Database"));
        }

        public DbSet<Post>? Posts { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Review>? Reviews { get; set; }
    }
}
