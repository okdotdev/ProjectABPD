using Microsoft.AspNetCore.Identity;

namespace abcAPI.Models;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        context.Database.EnsureCreated();


        if (context.Users.Any() || context.Clients.Any())
        {
            return;
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        User admin = new()
        {
            UserName = "admin", Nickname = "admin", IsAdmin = true, Email = "admin@example.com", EmailConfirmed = true
        };
        User normalUser = new()
        {
            UserName = "user", Nickname = "user", IsAdmin = false, Email = "user@example.com", EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "admin");
        await userManager.CreateAsync(normalUser, "user");

        await userManager.AddToRoleAsync(admin, "Admin");
        await userManager.AddToRoleAsync(normalUser, "User");


        Client[] clients =
        [
            new ClientIndividual
            {
                Address = "123 Main St", Email = "client1@example.com", PhoneNumber = "1234567890", FirstName = "John",
                LastName = "Doe", Pesel = "12345678901"
            },
            new ClientCompany
            {
                Address = "456 Elm St", Email = "client2@example.com", PhoneNumber = "0987654321",
                CompanyName = "XYZ Inc", Krs = "0987654321"
            }
        ];

        context.Clients.AddRange(clients);

        context.SaveChanges();
    }
}