using System.Collections.Generic;

namespace Domain.Entities
{
  public class PredefinedGroup
  {
    public int Id { get; set; }
    public ChannelSettings ChannelSettings { get; set; }
    public int ChannelSettingsId { get; set; }

    public ICollection<PredefinedGroupMember> PredefinedGroupMembers { get; set; }
  }
}
