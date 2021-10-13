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
    public string RoundStartChannelMessage { get; set; }
    public string RoundStartGroupMessage { get; set; }
    public string RoundMidwayMessage { get; set; }
    public string RoundFinisherMessage { get; set; }
    public int InitializeRoundHour { get; set; }
    public int MidwayRoundHour { get; set; }
    public int FinalizeRoundHour { get; set; }
  }
}
