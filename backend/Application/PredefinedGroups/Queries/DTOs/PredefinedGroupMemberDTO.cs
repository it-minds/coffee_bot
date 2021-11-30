using Application.Common.Mappings;
using Domain.Entities;

namespace Application.PredefinedGroups.DTOs
{
  public class PredefinedGroupMemberDTO : IAutoMap<PredefinedGroupMember>
  {
    public int Id { get; set; }
    public int PredefinedGroupId { get; set; }
    public int ChannelMemberId { get; set; }
  }
}
