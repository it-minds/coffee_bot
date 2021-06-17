using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ChannelMemberConfig : IEntityTypeConfiguration<ChannelMember>
  {
    public void Configure(EntityTypeBuilder<ChannelMember> builder)
    {
      builder.HasOne<ChannelSettings>(e => e.ChannelSettings)
          .WithMany(e => e.ChannelMembers)
          .HasForeignKey(e => e.ChannelSettingsId)
          .IsRequired(true);
    }
  }
}
