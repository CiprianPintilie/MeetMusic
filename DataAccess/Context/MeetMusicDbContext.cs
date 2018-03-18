using Data.Provider;
using MeetMusicModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Context
{
    public class MeetMusicDbContext : DbContext
    {
        public string ConnectionString { get; set; }


        public MeetMusicDbContext(DbContextOptions<MeetMusicDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); var fact = new LoggerFactory();
            fact.AddProvider(new SqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(fact);
        }

        public DbSet<User> Users { get; set; }

    }
}
