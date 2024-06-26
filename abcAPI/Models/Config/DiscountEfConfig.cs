using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class DiscountEfConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Type).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Value).IsRequired().HasColumnType("decimal(5,2)");
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();

        builder.ToTable("Discounts");
    }
}