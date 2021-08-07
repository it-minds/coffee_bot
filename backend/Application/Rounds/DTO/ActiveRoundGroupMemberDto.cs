using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Rounds.DTO
{
  public class ActiveRoundGroupMemberDto : IAutoMap<CoffeeRoundGroupMember>
  {
    public int Id { get; set; }
    public string SlackMemberId { get; set; }
    public string SlackMemberName { get; set; } //
    public bool Participated { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRoundGroupMember, ActiveRoundGroupMemberDto>()
        .ForMember(x => x.SlackMemberName, opts => opts.Ignore())
      ;
    }
  }
}
