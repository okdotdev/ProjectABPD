using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class ClientEfConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.IdClient);
        builder.Property(e => e.IdClient).UseIdentityColumn();

        builder.Property(e => e.Address).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
        builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(50);

        builder.Property(e => e.IsDeleted).IsRequired();
        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasDiscriminator<string>("ClientType")
            .HasValue<ClientIndividual>("Individual")
            .HasValue<ClientCompany>("Company");

        builder.Property<string>("ClientType").HasMaxLength(50);

        builder.ToTable("Clients");
    }
}

public class ClientIndividualEfConfig : IEntityTypeConfiguration<ClientIndividual>
{
    public void Configure(EntityTypeBuilder<ClientIndividual> builder)
    {
        builder.HasBaseType<Client>();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Pesel).IsRequired().HasMaxLength(50);
    }
}

public class ClientCompanyEfConfig : IEntityTypeConfiguration<ClientCompany>
{
    public void Configure(EntityTypeBuilder<ClientCompany> builder)
    {
        builder.HasBaseType<Client>();
        builder.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Krs).IsRequired().HasMaxLength(50);
    }
}
