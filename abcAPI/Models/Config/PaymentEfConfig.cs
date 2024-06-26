using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class PaymentEfConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");


        builder.HasOne(e => e.Contract)
            .WithMany(e => e.Payments)
            .HasForeignKey(e => e.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Payments");
    }
}