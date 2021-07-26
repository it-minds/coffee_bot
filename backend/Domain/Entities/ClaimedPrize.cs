using System;

namespace Domain.Entities
{
  public class ClaimedPrize {

    public int Id { get; set; }
    public int PointCost { get; set; }

    public int PrizeId { get; set; }
    public Prize Prize { get; set; }

    public DateTimeOffset DateClaimed { get; set; }

    public int ChannelMemberId { get; set; }
    public ChannelMember ChannelMember { get; set; }
  }
}
