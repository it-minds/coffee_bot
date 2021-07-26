using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ClaimedPrizeConfig : IEntityTypeConfiguration<ClaimedPrize>
  {
    public void Configure(EntityTypeBuilder<ClaimedPrize> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne<ChannelMember>(e => e.ChannelMember)
          .WithMany(e => e.ClaimedPrizes)
          .HasForeignKey(e => e.ChannelMemberId)
          .IsRequired(true);

      builder.HasOne<Prize>(e => e.Prize)
          .WithMany(e => e.ClaimedPrizes)
          .HasForeignKey(e => e.PrizeId)
          .IsRequired(true);

      builder.Property(x => x.PointCost);
    }
  }
}
