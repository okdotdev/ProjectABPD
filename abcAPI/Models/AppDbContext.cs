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

        var hasher = new PasswordHasher<User>();
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

        modelBuilder.Entity<ClientIndividual>().HasData(
            new ClientIndividual
            {
                IdClient = 1,
                Address = "123 Elm St",
                Email = "elmo@example.com",
                PhoneNumber = "555-555-5555",
                IsDeleted = false,
                FirstName = "Elmo",
                LastName = "Monster",
                Pesel = "55555555555"
            },
            new ClientIndividual
            {
                IdClient = 2,
                Address = "456 Maple Ave",
                Email = "mail@mail.com",
                PhoneNumber = "666-666-6666",
                IsDeleted = false,
                FirstName = "Eugene",
                LastName = "Raynor",
                Pesel = "66666666666"
            },
            new ClientIndividual
            {
                IdClient = 3,
                Address = "789 Oak Dr",
                Email = "john.doe@example.com",
                PhoneNumber = "555-123-4567",
                IsDeleted = false,
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901"
            },
            new ClientIndividual
            {
                IdClient = 4,
                Address = "321 Pine Ln",
                Email = "jane.smith@example.com",
                PhoneNumber = "555-987-6543",
                IsDeleted = false,
                FirstName = "Jane",
                LastName = "Smith",
                Pesel = "98765432109"
            }
        );

        modelBuilder.Entity<ClientCompany>().HasData(
            new ClientCompany
            {
                IdClient = 5,
                Address = "111 Birch Blvd",
                Email = "company1@example.com",
                PhoneNumber = "555-111-2222",
                IsDeleted = false,
                CompanyName = "Tech Solutions",
                Krs = "1234567890"
            },
            new ClientCompany
            {
                IdClient = 6,
                Address = "222 Cedar St",
                Email = "company2@example.com",
                PhoneNumber = "555-333-4444",
                IsDeleted = false,
                CompanyName = "Business Corp",
                Krs = "0987654321"
            }
        );

        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                Id = 1,
                Name = "Finance Pro",
                Description = "A comprehensive financial management software.",
                CurrentVersion = "1.0",
                Category = "Finance"
            },
            new Software
            {
                Id = 2,
                Name = "Edu Learn",
                Description = "An educational platform for online learning.",
                CurrentVersion = "2.5",
                Category = "Education"
            }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1,
                Name = "Black Friday",
                Type = "Subscription",
                Value = 0.10m,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 03, 03)
            },
            new Discount
            {
                Id = 2,
                Name = "New Year",
                Type = "Purchase",
                Value = 0.15m,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 01, 31)
            }
        );

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                SoftwareId = 1,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 12, 31),
                Price = 5000m,
                IsPaid = false,
                Version = "1.0",
                AdditionalSupportYears = 1,
                IsSigned = false
            },
            new Contract
            {
                Id = 2,
                SoftwareId = 2,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 12, 31),
                Price = 3000m,
                IsPaid = false,
                Version = "2.5",
                AdditionalSupportYears = 2,
                IsSigned = false
            }
        );

        modelBuilder.Entity<Subscription>().HasData(
            new Subscription
            {
                Id = 1,
                SoftwareId = 1,
                ClientId = 1,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 12, 31),
                Price = 100m,
                RenewalPeriod = "Monthly",
                IsActive = true
            },
            new Subscription
            {
                Id = 2,
                SoftwareId = 2,
                ClientId = 2,
                StartDate = new DateTime(2024, 01, 01),
                EndDate = new DateTime(2024, 12, 31),
                Price = 200m,
                RenewalPeriod = "Monthly",
                IsActive = true
            }
        );

        modelBuilder.Entity<ClientContract>().HasData(
            new ClientContract
            {
                Id = 1,
                ClientId = 3,
                ContractId = 1
            },
            new ClientContract
            {
                Id = 2,
                ClientId = 4,
                ContractId = 2
            }
        );

    }
}