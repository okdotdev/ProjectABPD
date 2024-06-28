using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class SubscriptionEfConfig : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.OfferName).IsRequired().HasMaxLength(100);

        builder.Property(e => e.PriceOfRenewal).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.IsMonthly).IsRequired().HasMaxLength(50);
        builder.Property(e => e.IsActive).IsRequired();

        builder.HasOne(e => e.Contract)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.ContractId);


        builder.ToTable("Subscriptions");
    }
}