using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Application.ChannelSync.Commands.ChannelMemberPointsSync.Extensions
{
  public static class ChannelMemberExtension
  {
    public static int RoundScore(this ChannelMember channelMember, IEnumerable<CoffeeRoundGroupMember> members )
    {
      return members
        .Where(x =>
          x.CoffeeRoundGroup.CoffeeRound.ChannelId == channelMember.ChannelSettingsId &&
          x.SlackMemberId == channelMember.SlackUserId && x.Participated
        )
        .Sum(x =>
        {
          var points = 0;
          if (x.CoffeeRoundGroup.HasMet) points += 1;
          if (x.CoffeeRoundGroup.HasPhoto) points += 2;
          if (x.CoffeeRoundGroup.FinishedAt < x.CoffeeRoundGroup.CoffeeRound.EndDate) points += 1;

          return points;
        });
    }
  }
}
