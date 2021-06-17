using System;
using System.Collections.Generic;

namespace Domain.Entities
{
  public class CoffeeRound
  {
    public int Id { get; set; }
    public ChannelSettings ChannelSettings { get; set; }
    public int ChannelId { get; set; }
    public string SlackChannelId { get; set; }

    public bool Active { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public ICollection<CoffeeRoundGroup> CoffeeRoundGroups { get; set; }
  }
}
