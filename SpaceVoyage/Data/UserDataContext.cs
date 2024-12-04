using Microsoft.EntityFrameworkCore;

namespace SpaceVoyage.Data
{
    public class UserDataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public UserDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("UserDB"));
        }

        public DbSet<User> Users { get; set; }
    }
}
