using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class CoffeeRoundGroupConfig : IEntityTypeConfiguration<CoffeeRoundGroup>
  {
    public void Configure(EntityTypeBuilder<CoffeeRoundGroup> builder)
    {
      builder.HasOne<CoffeeRound>(e => e.CoffeeRound)
          .WithMany(e => e.CoffeeRoundGroups)
          .HasForeignKey(e => e.CoffeeRoundId)
          .IsRequired(true);

      builder.Property(x => x.SlackPhotoUrl);
      builder.Property(x => x.LocalPhotoUrl);
    }
  }
}
