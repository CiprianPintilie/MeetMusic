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
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<MusicGenre>().HasOne(g => g.Family)
                .WithMany(f => f.Genres)
                .HasForeignKey(g => g.FamilyId);

            modelBuilder.Entity<UserMusicFamily>()
                .HasKey(uf => new {uf.UserId, uf.FamilyId});
            modelBuilder.Entity<UserMusicFamily>().HasOne(uf => uf.User)
                .WithMany(f => f.UserMusicFamilies)
                .HasForeignKey(uf => uf.UserId);
            modelBuilder.Entity<UserMusicFamily>()
                .HasOne(uf => uf.MusicFamily)
                .WithMany(f => f.UserMusicFamilies)
                .HasForeignKey(uf => uf.FamilyId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); var fact = new LoggerFactory();
            fact.AddProvider(new SqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(fact);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MusicFamily> MusicFamilies { get; set; }
        public DbSet<MusicGenre> MusicGenres { get; set; }
        public DbSet<UserMusicFamily> UserMusicFamilies { get; set; }
    }
}
