using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ChannelNoticeConfig : IEntityTypeConfiguration<ChannelNotice>
  {
    public void Configure(EntityTypeBuilder<ChannelNotice> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne<ChannelSettings>(x => x.ChannelSettings)
        .WithMany(x => x.ChannelNotices)
        .HasForeignKey(x => x.ChannelSettingsId);
    }
  }
}
