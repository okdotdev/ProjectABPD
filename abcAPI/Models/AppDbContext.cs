using abcAPI.Models.Config;
using abcAPI.Models.TableModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Models;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<ClientContract> ClientContracts { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientEfConfig).Assembly);

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );

        PasswordHasher<User> hasher = new();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null, "admin"),
                SecurityStamp = string.Empty
            },
            new User
            {
                Id = "2",
                UserName = "user",
                NormalizedUserName = "USER",
                PasswordHash = hasher.HashPassword(null, "user"),
                SecurityStamp = string.Empty
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = "1", RoleId = "1" },
            new IdentityUserRole<string> { UserId = "2", RoleId = "2" }
        );


    }
}