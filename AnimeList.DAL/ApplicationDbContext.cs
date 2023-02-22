using AnimeList.Domain.Chat;
using AnimeList.Domain.Entity;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
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
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FileModel> FileModels { get; set; } 

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

            modelBuilder.Entity<FileModel>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.FileModel)
                .HasForeignKey<UserProfile>(x => x.FileModelId);

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

            modelBuilder.Entity<News>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<News>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.News)
                .HasForeignKey(x => x.NewsId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.News)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
