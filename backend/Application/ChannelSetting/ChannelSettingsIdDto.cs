using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.ChannelSetting
{
  public class ChannelSettingsIdDto : ChannelSettingsDto
  {
    public int Id { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<ChannelSettings, ChannelSettingsIdDto>()
        .IncludeBase<ChannelSettings, ChannelSettingsDto>();
    }
  }
}
