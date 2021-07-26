using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class PrizeConfig : IEntityTypeConfiguration<Prize>
  {
    public void Configure(EntityTypeBuilder<Prize> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne<ChannelSettings>(e => e.ChannelSettings)
          .WithMany(e => e.Prizes)
          .HasForeignKey(e => e.ChannelSettingsId)
          .IsRequired(true);

      builder.Property(x => x.IsMilestone);
      builder.Property(x => x.IsRepeatable);
      builder.Property(x => x.PointCost);
    }
  }
}
