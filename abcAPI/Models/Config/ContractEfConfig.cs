using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class ContractEfConfig : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.SoftwareId).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.IsPaid).IsRequired();
        builder.Property(e => e.AdditionalSupportYears).IsRequired();
        builder.Property(e => e.IsSigned).IsRequired();

        builder.HasOne(e => e.Software)
            .WithMany(e => e.Contracts)
            .HasForeignKey(e => e.SoftwareId);

        builder.ToTable("Contracts");
    }
}