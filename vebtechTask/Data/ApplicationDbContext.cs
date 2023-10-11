using Microsoft.EntityFrameworkCore;
using vebtechTask.Models;

namespace vebtechTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<Admin>().HasData(
            new Admin[]
            {
            new Admin {
                Id = 1,
                Email = _configuration["AdminUser:Email"],
                Password = BCrypt.Net.BCrypt.HashPassword(_configuration["AdminUser:Password"])
            },
            });
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                new User { Id = 1, Name = "User 1", Age = 25, Email = "user1@example.com" },
                new User { Id = 2, Name = "User 2", Age = 30, Email = "user2@example.com" },
                new User { Id = 3, Name = "User 3", Age = 35, Email = "user3@example.com" },
                new User { Id = 4, Name = "User 4", Age = 40, Email = "user4@example.com" },
                new User { Id = 5, Name = "User 5", Age = 45, Email = "user5@example.com" },
                new User { Id = 6, Name = "User 6", Age = 50, Email = "user6@example.com" }
                });
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                new Role { Id = 1, Name = "User", UserId = 1 },
                new Role { Id = 2, Name = "Admin", UserId = 1 },
                new Role { Id = 3, Name = "Support", UserId = 1 },
                new Role { Id = 4, Name = "User", UserId = 2 },
                new Role { Id = 5, Name = "Admin", UserId = 2 },
                new Role { Id = 6, Name = "User", UserId = 3 },
                new Role { Id = 7, Name = "SuperAdmin", UserId = 3 },
                new Role { Id = 10, Name = "User", UserId = 4 },
                new Role { Id = 12, Name = "Support", UserId = 5 },
                new Role { Id = 13, Name = "Admin", UserId = 5 },
                new Role { Id = 16, Name = "User", UserId = 6 },
                new Role { Id = 17, Name = "Support", UserId = 6 },
                });
            modelBuilder.Entity<Role>()
            .HasOne(r => r.User)
            .WithMany(u => u.Roles)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
