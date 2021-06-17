using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class CoffeeRoundGroupMemberConfig : IEntityTypeConfiguration<CoffeeRoundGroupMember>
  {
    public void Configure(EntityTypeBuilder<CoffeeRoundGroupMember> builder)
    {
      builder.HasOne<CoffeeRoundGroup>(e => e.CoffeeRoundGroup)
          .WithMany(e => e.CoffeeRoundGroupMembers)
          .HasForeignKey(e => e.CoffeeRoundGroupId)
          .IsRequired(true);
    }
  }
}
