using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Rounds.DTO
{
  public class RoundSnipDto : IAutoMap<CoffeeRound>
  {
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public string SlackChannelId { get; set; }
    public bool Active { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }


    public decimal MeetupPercentage { get; set; }
    public decimal PhotoPercentage { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRound, RoundSnipDto>()
        .ForMember(x => x.MeetupPercentage, opts => opts.MapFrom(round => round.CoffeeRoundGroups.Percent(group => group.HasMet)))
        .ForMember(x => x.PhotoPercentage, opts => opts.MapFrom(round => round.CoffeeRoundGroups.Percent(group => group.HasPhoto,group => group.HasMet)))
      ;
    }
  }
}
