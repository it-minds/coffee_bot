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
          x.SlackMemberId == channelMember.SlackUserId
        )
        .Sum(member =>
        {
          if (member.CoffeeRoundGroup.CoffeeRound.Active) return 0;
          if (!member.Participated) return -1;
          var points = 0;
          if (member.CoffeeRoundGroup.HasMet) points += 1;
          if (member.CoffeeRoundGroup.HasPhoto) points += 2;
          if (member.CoffeeRoundGroup.FinishedAt < member.CoffeeRoundGroup.CoffeeRound.EndDate) points += 1;

          return points;
        });
    }
  }
}
