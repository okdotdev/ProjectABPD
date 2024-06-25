using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class UserEfConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
        builder.Property(e => e.IsAdmin).IsRequired();

        builder.ToTable("Users");
    }
}