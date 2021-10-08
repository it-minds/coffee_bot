using Domain.Defaults;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ChannelSettingsConfig : IEntityTypeConfiguration<ChannelSettings>
  {
    public void Configure(EntityTypeBuilder<ChannelSettings> builder)
    {
      builder.Property(e => e.RoundStartChannelMessage)
        .IsRequired()
        .HasDefaultValue(ChannelMessageDefaults.RoundStartChannelMessage);

      builder.Property(e => e.RoundStartGroupMessage)
        .IsRequired()
        .HasDefaultValue(ChannelMessageDefaults.RoundStartGroupMessage);

      builder.Property(e => e.RoundMidwayMessage)
        .IsRequired()
        .HasDefaultValue(ChannelMessageDefaults.RoundMidwayMessage);

      builder.Property(e => e.RoundFinisherMessage)
        .IsRequired()
        .HasDefaultValue(ChannelMessageDefaults.RoundFinisherMessage);
    }
  }
}
