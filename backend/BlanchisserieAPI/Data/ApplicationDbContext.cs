using Microsoft.EntityFrameworkCore;
using BlanchisserieAPI.Models;

namespace BlanchisserieAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index unique pour éviter les doublons utilisateur-rôle
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // Index unique pour les noms d'utilisateur
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Index unique pour les emails
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index unique pour les noms de rôles
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Données de démarrage
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Création des rôles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Utilisateur" }
            );

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Création des utilisateurs
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    Password = "admin123", // En production, il faudrait hacher le mot de passe
                    Email = "admin@blanchisserie.com",
                    FirstName = "Administrateur",
                    LastName = "Système",
                    CreatedAt = seedDate,
                    IsActive = true
                },
                new User 
                { 
                    Id = 2, 
                    Username = "user", 
                    Password = "user123", // En production, il faudrait hacher le mot de passe
                    Email = "user@blanchisserie.com",
                    FirstName = "Utilisateur",
                    LastName = "Test",
                    CreatedAt = seedDate,
                    IsActive = true
                }
            );

            // Attribution des rôles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = 1, RoleId = 1, AssignedAt = seedDate }, // admin -> Admin
                new UserRole { Id = 2, UserId = 2, RoleId = 2, AssignedAt = seedDate }  // user -> Utilisateur
            );
        }
    }
}
