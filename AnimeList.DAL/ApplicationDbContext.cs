using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AnimeList.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }   
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AnimeGenre> AnimeGenres { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<UserAnimeList> UserAnimeLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.Profile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfile>(x => x.UserId);

            modelBuilder.Entity<UserProfile>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.UserId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.AnimeList)
                .WithOne(x => x.Profile)
                .IsRequired();

            modelBuilder.Entity<AnimeGenre>()
                .HasKey(ag => new { ag.AnimeId, ag.GenreId });
            modelBuilder.Entity<AnimeGenre>()
                .HasOne(ag => ag.Anime)
                .WithMany(x => x.AnimeGenres)
                .HasForeignKey(x => x.AnimeId);
            modelBuilder.Entity<AnimeGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(x => x.AnimeGenre)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<Anime>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Title).IsRequired();
                builder.Property(x => x.AnimeType).IsRequired();
            });

            modelBuilder.Entity<UserAnimeList>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
