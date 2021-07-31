using System;
using System.Collections.Generic;

namespace Domain.Entities
{
  public class ChannelMember
  {
    public int Id { get; set; }

    public string SlackUserId { get; set; }
    public string SlackName { get; set; }

    public int Points { get; set; }

    public int ChannelSettingsId { get; set; }
    public ChannelSettings ChannelSettings { get; set; }

    public bool IsAdmin { get; set; } = false;
    public bool OnPause { get; set; } = false;
    public bool IsRemoved { get; set; } = false;

    public DateTimeOffset? ReturnFromPauseDate { get; set; }

    public ICollection<ClaimedPrize> ClaimedPrizes { get; set; }
  }
}
