using System.Collections.Generic;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.PredefinedGroups.DTOs
{
  public class PredefinedGroupDTO : IAutoMap<PredefinedGroup>
  {
    public int Id { get; set; }
    public int ChannelSettingsId { get; set; }
    public IEnumerable<PredefinedGroupMemberDTO> PredefinedGroupMembers { get; set; }

    // public void Mapping(Profile profile)
    // {
    //   profile.CreateMap<PredefinedGroup, PredefinedGroupDTO>()
    //     .ForMember(x => x.PredefinedGroupMembers
    //   ;
    // }
  }
}
