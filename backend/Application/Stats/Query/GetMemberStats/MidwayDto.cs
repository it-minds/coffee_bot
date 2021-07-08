using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Stats.Query.GetMemberStats
{
  public class MidwayDto: IAutoMap<CoffeeRoundGroupMember>
  {
    public string SlackMemberId { get; set; }
    public bool HasMet { get; set; }
    public bool HasPhoto { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRoundGroupMember, MidwayDto>()
        .ForMember(x => x.HasMet, opts => opts.MapFrom(from => from.CoffeeRoundGroup.HasMet))
        .ForMember(x => x.HasPhoto, opts => opts.MapFrom(from => !String.IsNullOrEmpty(from.CoffeeRoundGroup.PhotoUrl) ))

      ;

    }
  }
}
