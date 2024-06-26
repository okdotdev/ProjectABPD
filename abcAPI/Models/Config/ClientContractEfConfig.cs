using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace abcAPI.Models.Config;

public class ClientContractEfConfig : IEntityTypeConfiguration<ClientContract>
{
    public void Configure(EntityTypeBuilder<ClientContract> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.HasOne(e => e.Client)
            .WithMany(e => e.ClientContracts)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(cc => !cc.Client.IsDeleted);

        builder.HasOne(e => e.Contract)
            .WithMany(e => e.ClientContracts)
            .HasForeignKey(e => e.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ClientContracts");
    }
}