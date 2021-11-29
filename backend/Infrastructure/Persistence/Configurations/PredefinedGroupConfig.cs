using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class PredefinedGroupConfig : IEntityTypeConfiguration<PredefinedGroup>
  {
    public void Configure(EntityTypeBuilder<PredefinedGroup> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne<ChannelSettings>(x => x.ChannelSettings);
    }
  }
}
