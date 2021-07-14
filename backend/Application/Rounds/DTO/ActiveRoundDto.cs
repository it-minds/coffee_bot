using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Rounds.DTO
{
  public class ActiveRoundDto : IAutoMap<CoffeeRound>
  {
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public string SlackChannelId { get; set; }
    public bool Active { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public IEnumerable<ActiveRoundGroupDto> Groups { get; set; }

    public decimal? PreviousMeetup { get; set; }
    public decimal? PreviousPhoto { get; set; }

    public int? PreviousId { get; set; }
    public int? NextId { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRound, ActiveRoundDto>()
        .ForMember(x => x.Groups, opts => opts.MapFrom(y => y.CoffeeRoundGroups))
        .ForMember(x => x.PreviousMeetup, opts => opts.Ignore())
        .ForMember(x => x.PreviousPhoto, opts => opts.Ignore())
        .ForMember(x => x.PreviousId, opts => opts.Ignore())
        .ForMember(x => x.NextId, opts => opts.Ignore())
      ;
    }
  }
}
