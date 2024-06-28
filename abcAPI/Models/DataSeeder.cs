using Bogus;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Models;

public static class DataSeeder
{
    public static void SeedData(IServiceProvider serviceProvider)
    {
        using AppDbContext context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Clients.Any()) return;
        Faker<Client>? clientFaker = new Faker<Client>()
            .RuleFor(c => c.Address, f => f.Address.StreetAddress())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.IsDeleted, f => false);

        Faker<ClientIndividual>? individualClientFaker = new Faker<ClientIndividual>()
            .RuleFor(ic => ic.FirstName, f => f.Name.FirstName())
            .RuleFor(ic => ic.LastName, f => f.Name.LastName())
            .RuleFor(ic => ic.Pesel, f => f.Random.ReplaceNumbers("###########"))
            .RuleFor(ic => ic.Address, f => f.Address.StreetAddress())
            .RuleFor(ic => ic.Email, f => f.Internet.Email())
            .RuleFor(ic => ic.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(ic => ic.IsDeleted, f => false);

        Faker<ClientCompany>? companyClientFaker = new Faker<ClientCompany>()
            .RuleFor(cc => cc.CompanyName, f => f.Company.CompanyName())
            .RuleFor(cc => cc.Krs, f => f.Random.ReplaceNumbers("##########"))
            .RuleFor(cc => cc.Address, f => f.Address.StreetAddress())
            .RuleFor(cc => cc.Email, f => f.Internet.Email())
            .RuleFor(cc => cc.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(cc => cc.IsDeleted, f => false);

        List<Client>? clients = clientFaker.Generate(10);
        List<ClientIndividual>? individualClients = individualClientFaker.Generate(5);
        List<ClientCompany>? companyClients = companyClientFaker.Generate(5);

        context.Clients.AddRange(clients);
        context.Clients.AddRange(individualClients);
        context.Clients.AddRange(companyClients);
        context.SaveChanges();


    }
}