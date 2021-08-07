using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using Application.Rounds.Extensions;
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
    public string LocalPhotoUrl { get; set; }
    public int CoffeeRoundId { get; set; }
    public IEnumerable<ActiveRoundGroupMemberDto> CoffeeRoundGroupMembers { get; set; }


    public int GroupScore { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<CoffeeRoundGroup, ActiveRoundGroupDto>()
        .ForMember(x => x.GroupScore, opts => opts.MapFrom(y => y.GroupScore()))
      ;
    }
  }
}
