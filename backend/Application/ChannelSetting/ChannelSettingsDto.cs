using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.ChannelSetting
{
  public class ChannelSettingsDto : IAutoMap<ChannelSettings>
  {
    public int GroupSize { get; set; }
    public DayOfWeek StartsDay { get; set; }
    public int WeekRepeat { get; set; }
    public int DurationInDays { get; set; }
    public bool IndividualMessage { get; set; }
    public string SlackChannelId { get; set; }
    public string SlackChannelName { get; set; }
  }
}
