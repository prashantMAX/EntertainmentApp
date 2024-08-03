using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EntertainmentApp.Infrastructure.Identity;
using EntertainmentApp.Domain.Entities;

namespace EntertainmentApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entertainment> Entertainments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserLike> UserLikes { get; set; } // Add UserLikes DbSet

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Global query filters for soft delete
            builder.Entity<Entertainment>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Review>().HasQueryFilter(r => !r.IsDeleted);
            builder.Entity<Creator>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Genre>().HasQueryFilter(g => !g.IsDeleted);
            builder.Entity<UserLike>().HasQueryFilter(ul => !ul.IsDeleted);

            // Configure Review relationships
            builder.Entity<Review>()
                .HasOne(r => r.Entertainment)
                .WithMany(e => e.Reviews)
                .HasForeignKey(r => r.EntertainmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure UserLike relationships
            builder.Entity<UserLike>()
                .HasOne(ul => ul.Review)
                .WithMany(r => r.UserLikes)
                .HasForeignKey(ul => ul.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserLike>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Entertainment relationships
            builder.Entity<Entertainment>()
                .HasOne(e => e.Genre)
                .WithMany(g => g.Entertainments)
                .HasForeignKey(e => e.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Entertainment>()
                .HasOne(e => e.Creator)
                .WithMany(c => c.Entertainments)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            builder.Entity<Genre>().HasData(
               new Genre { Id = 1, Name = "Adventure" },
               new Genre { Id = 2, Name = "Action" },
               new Genre { Id = 3, Name = "Scifi" },
               new Genre { Id = 4, Name = "Thriller" },
               new Genre { Id = 5, Name = "Drama" },
               new Genre { Id = 6, Name = "Horror" },
               new Genre { Id = 7, Name = "Documentary" },
               new Genre { Id = 8, Name = "Comedy" },
               new Genre { Id = 9, Name = "History" },
               new Genre { Id = 10, Name = "Mystery" }
           );

            builder.Entity<Creator>().HasData(
              new Creator { Id = 1, Name = "Steven Spielberg" },
              new Creator { Id = 2, Name = "Martin Scorsese" },
              new Creator { Id = 3, Name = "Christopher Nolan" },
              new Creator { Id = 4, Name = "Sidney Lumet" },
              new Creator { Id = 5, Name = "Hayao Miyazaki" },
              new Creator { Id = 6, Name = "Ubisoft" },
              new Creator { Id = 7, Name = "Terry Gilliam" },
              new Creator { Id = 8, Name = "EA ENTERTAINMENT" },
              new Creator { Id = 9, Name = "RIOT GAMES" },
              new Creator { Id = 10, Name = "Bong Joon-Ho" }
          );
        }
    }
}
