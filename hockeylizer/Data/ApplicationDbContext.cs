using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using hockeylizer.Models;

namespace hockeylizer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerSession> Sessions { get; set; }

        public DbSet<Target> Targets { get; set; }

        public DbSet<AppTeam> AppTeams { get; set; }

        public DbSet<FrameToAnalyze> Frames { get; set; }

        public DbSet<AimPoint> AimPoints { get; set; }
    }
}
