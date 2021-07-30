using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class CoffeeRoundConfig : IEntityTypeConfiguration<CoffeeRound>
  {
    public void Configure(EntityTypeBuilder<CoffeeRound> builder)
    {
      builder.HasOne<ChannelSettings>(e => e.ChannelSettings)
        .WithMany(e => e.CoffeeRounds)
        .HasForeignKey(e => e.ChannelId);
    }
  }
}
