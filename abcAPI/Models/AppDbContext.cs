using abcAPI.Models.Config;
using abcAPI.Models.TableModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Models;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Client> Clients { get; set; }

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

// Seed clients
        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                IdClient = 1, Address = "123 Main St", Email = "client1@example.com", PhoneNumber = "123-456-7890",
                IsDeleted = false
            },
            new Client
            {
                IdClient = 2, Address = "456 Maple Ave", Email = "client2@example.com", PhoneNumber = "987-654-3210",
                IsDeleted = false
            }
        );

        // Seed individual clients
        modelBuilder.Entity<ClientIndividual>().HasData(
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

        // Seed company clients
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
    }
}