using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common
{
  public class StandardGroupDto : IAutoMap<CoffeeRoundGroup>
  {
    public int Id { get; set; }
    public bool HasMet { get; set; }
    public bool HasPhoto { get; set; }
    public string PhotoUrl { get; set; }
    public DateTimeOffset FinishedAt { get; set; }

    public List<string> Members { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<CoffeeRoundGroup, StandardGroupDto>()
        .ForMember(x => x.Members, opts => opts.MapFrom(x => x.CoffeeRoundGroupMembers.Select( y => y.SlackMemberId)))
      ;
    }
  }
}
