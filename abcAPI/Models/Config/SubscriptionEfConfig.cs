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

        builder.Property(e => e.SoftwareId).IsRequired();
        builder.Property(e => e.ClientId).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.RenewalPeriod).IsRequired().HasMaxLength(50);
        builder.Property(e => e.IsActive).IsRequired();

        builder.HasOne(e => e.Software)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.SoftwareId);

        builder.HasOne(e => e.Client)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.Client.IsDeleted);

        builder.ToTable("Subscriptions");
    }
}