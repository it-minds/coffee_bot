using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class CoffeeRoundConfig : IEntityTypeConfiguration<CoffeeRound>
  {
    public void Configure(EntityTypeBuilder<CoffeeRound> builder)
    {
      builder.Property(e => e.SlackChannelId)
          .HasMaxLength(200)
          .IsRequired();
    }
  }
}
