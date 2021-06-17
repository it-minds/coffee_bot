using System;
using System.Collections.Generic;

namespace Domain.Entities
{
  public class ChannelSettings
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; }
    public string SlackChannelName { get; set; }

    public int GroupSize { get; set; } = 3;
    public DayOfWeek StartsDay { get; set; } = DayOfWeek.Monday;
    public int WeekRepeat { get; set; } = 2;
    public int DurationInDays { get; set; } = 11;
    public bool IndividualMessage { get; set; } = false;

    public ICollection<ChannelMember> ChannelMembers { get; set; }
  }
}
