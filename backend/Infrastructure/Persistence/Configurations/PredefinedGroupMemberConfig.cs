using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class PredefinedGroupMemberConfig : IEntityTypeConfiguration<PredefinedGroupMember>
  {
    public void Configure(EntityTypeBuilder<PredefinedGroupMember> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne<PredefinedGroup>(x => x.PredefinedGroup)
        .WithMany(x => x.PredefinedGroupMembers)
        .HasForeignKey(x => x.PredefinedGroupId)
        .IsRequired(true)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne<ChannelMember>(x => x.ChannelMember);
    }
  }
}
