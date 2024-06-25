namespace abcAPI.Models.Config;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any clients.
        if (context.IndividualClients.Any() && context.CorporateClients.Any())
        {
            return;   // DB has been seeded
        }

        IndividualClient[] individualClients = new IndividualClient[]
        {
            new IndividualClient{FirstName="Carson",LastName="Alexander", Address="123 Main St", Email="carson@example.com", PhoneNumber="123-456-7890", PESEL="12345678901"},
            new IndividualClient{FirstName="Meredith",LastName="Alonso", Address="456 Oak St", Email="meredith@example.com", PhoneNumber="098-765-4321", PESEL="23456789012"},
        };

        foreach (IndividualClient i in individualClients)
        {
            context.IndividualClients.Add(i);
        }

        CorporateClient[] corporateClients = new CorporateClient[]
        {
            new CorporateClient{CompanyName="Tech Corp", Address="789 Pine St", Email="info@techcorp.com", PhoneNumber="111-222-3333", KRS="9876543210"},
            new CorporateClient{CompanyName="Business Inc", Address="321 Cedar St", Email="contact@businessinc.com", PhoneNumber="444-555-6666", KRS="8765432109"},
        };

        foreach (CorporateClient c in corporateClients)
        {
            context.CorporateClients.Add(c);
        }

        context.SaveChanges();
    }
}