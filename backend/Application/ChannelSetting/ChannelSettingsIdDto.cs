using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.ChannelSetting
{
  public class ChannelSettingsIdDto : ChannelSettingsDto
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; }
    public string SlackChannelName { get; set; }
    public bool Paused { get; set; }


    public void Mapping(Profile profile) {
      profile.CreateMap<ChannelSettings, ChannelSettingsIdDto>()
        .IncludeBase<ChannelSettings, ChannelSettingsDto>()
        .ForMember(x => x.Paused, opts => opts.Ignore());
    }
  }
}
