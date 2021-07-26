using System.Linq;
using Application.Common.Linq;
using Domain.Entities;

namespace Application.Common.EntityExtentions
{
  public static class ChannelMemberExtenstions
  {
    public static int PointsUsed(this ChannelMember member) =>
      member.ClaimedPrizes.Where(x => !x.Prize.IsMilestone).Sum(x => x.PointCost);


    public static int PointsRemaining(this ChannelMember member) => member.Points - member.PointsUsed();


    public static bool CanAffordPrize(this ChannelMember member, Prize prize) =>
     (prize.IsMilestone ? member.Points : member.PointsRemaining() ) >= prize.PointCost;


    /// <summary>
    /// Check if the member is able to claim a prize by checking if
    /// the milestone has already been reached or if none repeatable already been claimed.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="prize"></param>
    /// <returns></returns>
    public static bool CanClaimPrize(this ChannelMember member, Prize prize) =>
      prize.IsRepeatable || member.ClaimedPrizes.Where(x => x.Prize.IsMilestone == prize.IsMilestone && !x.Prize.IsRepeatable).None(x => x.PrizeId == prize.Id );

    public static bool CanClaimAndAffordPrize(this ChannelMember member, Prize prize) =>
      member.CanClaimPrize(prize) && member.CanAffordPrize(prize);

  }
}
