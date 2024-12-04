using Microsoft.EntityFrameworkCore;

namespace SpaceVoyage.Data
{
    public class PatchnoteDataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public PatchnoteDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("PatchnoteDB"));
        }

        public DbSet<Patchnote> PatchNotes { get; set; }
    }
}
