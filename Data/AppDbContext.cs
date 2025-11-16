using Microsoft.EntityFrameworkCore;
using MyApp.Models;
namespace MyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchResult> MatchResults { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamA)
                .WithMany() // Assuming Team entity doesn't have a specific collection for this side
                .HasForeignKey(m => m.TeamAId)
                // Use DeleteBehavior.Restrict or DeleteBehavior.NoAction
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamB)
                .WithMany()
                .HasForeignKey(m => m.TeamBId)
                // Use DeleteBehavior.Restrict or DeleteBehavior.NoAction
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchResult>()
       .HasOne(r => r.Match)
       .WithOne()
       .HasForeignKey<MatchResult>(r => r.MatchId)
       .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
