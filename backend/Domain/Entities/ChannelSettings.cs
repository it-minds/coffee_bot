using System;
using System.Collections.Generic;
using Domain.Defaults;

namespace Domain.Entities
{
  public class ChannelSettings
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; } // SlackChannelId
    public string SlackChannelName { get; set; }
    public string SlackWorkSpaceId { get; set; }

    public int GroupSize { get; set; } = 3;
    public DayOfWeek StartsDay { get; set; } = DayOfWeek.Monday;
    public int WeekRepeat { get; set; } = 2;
    public int DurationInDays { get; set; } = 11;
    public bool IndividualMessage { get; set; } = false;

    public string RoundStartChannelMessage { get; set; } =  ChannelMessageDefaults.RoundStartChannelMessage;
    public string RoundStartGroupMessage { get; set; } =  ChannelMessageDefaults.RoundStartGroupMessage;
    public string RoundMidwayMessage { get; set; } =  ChannelMessageDefaults.RoundMidwayMessage;
    public string RoundFinisherMessage { get; set; } =  ChannelMessageDefaults.RoundFinisherMessage;
    public int InitializeRoundHour { get; set; } = 10;
    public int MidwayRoundHour { get; set; } = 11;
    public int FinalizeRoundHour { get; set; } = 16;

    public ICollection<ChannelMember> ChannelMembers { get; set; }
    public ICollection<CoffeeRound> CoffeeRounds { get; set; }
    public ICollection<Prize> Prizes { get; set; }
    public ICollection<ChannelNotice> ChannelNotices { get; set; }
  }
}
