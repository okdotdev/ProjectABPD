using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class SoftwareEfConfig : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(100);
        builder.Property(e => e.CurrentVersion).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Category).IsRequired().HasMaxLength(50);

        builder.ToTable("Softwares");
    }
}