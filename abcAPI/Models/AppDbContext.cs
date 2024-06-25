using abcAPI.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<IndividualClient> IndividualClients { get; set; }
    public DbSet<CorporateClient> CorporateClients { get; set; }

    public DbSet<User> Users { get; set; }


    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CorporateClientEFConfig).Assembly);
    }
}