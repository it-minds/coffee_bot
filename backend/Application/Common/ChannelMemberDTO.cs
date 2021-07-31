using System;
using Application.ChannelSetting;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Common
{
  public class ChannelMemberDTO : IAutoMap<ChannelMember>
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; } // no mapped
    public string SlackUserId { get; set; }
    public string SlackName { get; set; }
    public int ChannelSettingsId { get; set; }

    public int Points { get; set; }
    public bool IsAdmin { get; set; }
    public bool OnPause { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? ReturnFromPauseDate { get; set; }

    public ChannelSettingsIdDto ChannelSettings { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<ChannelMember, ChannelMemberDTO>()
        .ForMember(x => x.SlackChannelId, opts => opts.Ignore());
    }
  }
}
