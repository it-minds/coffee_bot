using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Rounds.DTO
{
  public class ActiveRoundGroupDto : IAutoMap<CoffeeRoundGroup>
  {
    public int Id { get; set; }
    public string SlackMessageId { get; set; }
    public bool HasMet { get; set; }
    public bool HasPhoto { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public int NotificationCount { get; set; }
    public string PhotoUrl { get; set; }
    public int CoffeeRoundId { get; set; }

    public IList<string> Members { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRoundGroup, ActiveRoundGroupDto>()
        .ForMember(x => x.Members, opts => opts.MapFrom(y => y.CoffeeRoundGroupMembers.Select(member => member.SlackMemberId)))
        .ForMember(x => x.PhotoUrl, opts => opts.MapFrom(y => y.LocalPhotoUrl))
      ;
    }
  }
}
