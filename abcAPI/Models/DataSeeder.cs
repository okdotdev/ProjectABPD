using abcAPI.Models.TableModels;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Models;

public static class DataSeeder
{
    public static void SeedData(IServiceProvider serviceProvider)
    {
        using AppDbContext context =
            new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Clients.Any()) return;

        Faker<ClientIndividual> individualClientFaker = new Faker<ClientIndividual>()
            .RuleFor(ic => ic.FirstName, f => f.Name.FirstName())
            .RuleFor(ic => ic.LastName, f => f.Name.LastName())
            .RuleFor(ic => ic.Pesel, f => f.Random.ReplaceNumbers("###########"))
            .RuleFor(ic => ic.Address, f => f.Address.StreetAddress())
            .RuleFor(ic => ic.Email, f => f.Internet.Email())
            .RuleFor(ic => ic.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(ic => ic.IsDeleted, f => false);

        Faker<ClientCompany> companyClientFaker = new Faker<ClientCompany>()
            .RuleFor(cc => cc.CompanyName, f => f.Company.CompanyName())
            .RuleFor(cc => cc.Krs, f => f.Random.ReplaceNumbers("##########"))
            .RuleFor(cc => cc.Address, f => f.Address.StreetAddress())
            .RuleFor(cc => cc.Email, f => f.Internet.Email())
            .RuleFor(cc => cc.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(cc => cc.IsDeleted, f => false);

        Faker<Software> softwareFaker = new Faker<Software>()
            .RuleFor(s => s.Name, f => f.Commerce.ProductName())
            .RuleFor(s => s.Description, f => f.Lorem.Paragraph())
            .RuleFor(s => s.CurrentVersion, f => f.Random.ReplaceNumbers("##.##.##"))
            .RuleFor(s => s.Category, f => f.Commerce.Department());

        List<Software> softwares = softwareFaker.Generate(124);
        context.Softwares.AddRange(softwares);
        context.SaveChanges();

        List<int> existingSoftwareIds = context.Softwares.Select(s => s.Id).ToList();

        Faker<Contract> contractFaker = new Faker<Contract>()
            .RuleFor(c => c.SoftwareId, f => f.PickRandom(existingSoftwareIds))
            .RuleFor(c => c.StartDate, f => f.Date.Past())
            .RuleFor(c => c.EndDate, f => f.Date.Future())
            .RuleFor(c => c.Price, f => f.Random.Decimal(100, 10000))
            .RuleFor(c => c.AmountPaid, f => f.Random.Decimal(0, 10000))
            .RuleFor(c => c.Version, f => f.Random.ReplaceNumbers("##.##.##"))
            .RuleFor(c => c.AdditionalSupportYears, f => f.Random.Number(0, 5))
            .RuleFor(c => c.IsSigned, f => f.Random.Bool())
            .FinishWith((f, c) => c.IsPaid = c.AmountPaid > 0);

        List<Contract> contracts = contractFaker.Generate(600);
        context.Contracts.AddRange(contracts);
        context.SaveChanges();


        List<ClientIndividual> individualClients = individualClientFaker.Generate(328);
        List<ClientCompany> companyClients = companyClientFaker.Generate(571);

        context.Clients.AddRange(individualClients);
        context.Clients.AddRange(companyClients);

        context.SaveChanges();
    }
}
