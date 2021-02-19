using System;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserEmail> UserEmails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Created = DateTimeOffset.Now,
                    Id = 1,
                    Name = "User"
                },
                new Role
                {
                    Created = DateTimeOffset.Now,
                    Id = 2,
                    Name = "Admin"
                }

            );
        }
    }
}
