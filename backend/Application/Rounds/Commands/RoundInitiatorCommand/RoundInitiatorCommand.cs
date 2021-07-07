using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;

namespace Rounds.Commands.RoundInitiatorCommand
{

  /// <summary>
  ///  This Command
  /// </summary>
  public class RoundInitiatorCommand
   : IRequest<int>
  {

    public class RoundInitiatorCommandHandler : IRequestHandler<RoundInitiatorCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public RoundInitiatorCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext) {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(RoundInitiatorCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await applicationDbContext.CoffeeRounds
          .Where(x => x.Active)
          .Select(x => x.SlackChannelId)
          .ToListAsync();

        var channelSettings = await applicationDbContext.ChannelSettings
          .Include(x => x.ChannelMembers)
          .Where(x => !activeRounds.Contains(x.SlackChannelId))
          .ToListAsync();

        foreach (var settings in channelSettings)
        {
          if (settings.ChannelMembers.Count() <= 0) {
            continue;
          }

          var membersToParticipate = settings.ChannelMembers.Where(x => !x.OnPause).Select(x => x.SlackUserId);
          var groups = SplitChannelIntoSubGroups(membersToParticipate, settings.GroupSize);

          var round = BuildNewCoffeeRound(settings);

          var channelMessage = BuildChannelMessage(settings, round, groups);

          await slackClient.SendMessageToChannel(cancellationToken, settings.SlackChannelId, channelMessage);

          var groupTasks = groups.Select(group => BuildNewCoffeeRoundGroup(settings: settings, round: round, group: group, cancellationToken: cancellationToken)).ToArray();

          await Task.WhenAll(groupTasks);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }

      private async Task<CoffeeRoundGroup> BuildNewCoffeeRoundGroup(ChannelSettings settings, CoffeeRound round, IEnumerable<string> group, CancellationToken cancellationToken)
      {
        var groupMessage = BuildGroupMessage(settings, round, group);

        var roundGroup = new CoffeeRoundGroup
        {
          CoffeeRound = round,
          CoffeeRoundGroupMembers = group.Select(x => new CoffeeRoundGroupMember
          {
            SlackMemberId = x
          }).ToList()
        };

        if (settings.IndividualMessage)
        {
          var pm = await slackClient.SendPrivateMessageToMembers(cancellationToken, group, groupMessage);
          roundGroup.SlackMessageId = pm.ChannelId;
        }

        applicationDbContext.CoffeeRoundGroups.Add(roundGroup);

        return roundGroup;
      }

      private CoffeeRound BuildNewCoffeeRound(ChannelSettings settings)
      {
        var baseDate = DateTimeOffset.UtcNow.Date;
        var daysToAdd = ((int) settings.StartsDay - (int) baseDate.DayOfWeek + 7) % 7;
        var startDate = baseDate.AddDays(daysToAdd);
        var endDate = startDate.AddDays(settings.DurationInDays);

        var round = new CoffeeRound {
          SlackChannelId = settings.SlackChannelId,
          Active = true,
          ChannelId = settings.Id,
          StartDate = startDate,
          EndDate = endDate
        };

        applicationDbContext.CoffeeRounds.Add(round);

        return round;
      }

      private string BuildGroupMessage(ChannelSettings settings, CoffeeRound round, IEnumerable<string> group)
      {
        var sb = new StringBuilder();
        sb.AppendLine("Time for your coffe!");

        sb.Append("The round starts: ")
          .Append(round.StartDate.ToString("dddd, dd/MMMM"))
          .Append(". The round ends: ")
          .Append(round.EndDate.ToString("dddd, dd/MMMM"))
          .AppendLine(".")
          .AppendLine("Have fun!");


        return sb.ToString();
      }

      private string BuildChannelMessage(ChannelSettings settings, CoffeeRound round, IEnumerable<IEnumerable<string>> groups)
      {
        var sb = new StringBuilder();
        sb.AppendLine("Time to drink coffee <!channel>");

        sb.Append("The round starts: ")
          .Append(round.StartDate.ToString("dddd, dd/MMMM"))
          .Append(". The round ends: ")
          .Append(round.EndDate.ToString("dddd, dd/MMMM"))
          .AppendLine(".");

        sb.AppendLine("The groups are:");
        for (int i = 0; i < groups.Count(); i++)
        {
          var group = groups.ToList()[i];

          sb.Append("Group "+(i+1))
            .Append(": ")
            .AppendJoin(", ", group.Select(x => "<@" + x + ">" ))
            .AppendLine("");
        }

        return sb.ToString();
      }

      /// <summary>
      /// Shuffles the input set and splits it into smaller groups by the given input size.
      /// <example><br/>example with incomplete group:
      /// <code>
      /// [1,2,3,4,5,6,7,8] => [[4,7,2],[1,8,5],[6,3]]
      /// </code>
      /// </example>
      /// <example>example with overflow group:
      /// <code>
      /// [1,2,3,4,5,6,7,8,9,10] => [[4,7,2,9],[1,8,5],[6,3,10]]
      /// </code>
      /// </example>
      /// </summary>
      /// <param name="members"></param>
      /// <param name="groupSizes"></param>
      /// <returns>An <c>IEnumerable</c> in chunks of <c>IEnumerable</c> by the shuffled input</returns>
      private IEnumerable<IEnumerable<string>> SplitChannelIntoSubGroups(IEnumerable<string> members, int groupSizes = 3)
      {
        // Number representing how many groups there should be.
        var calculatedChunkCount = (int) Math.Floor((decimal) members.Count() / groupSizes);

        // Minimum size of any group is half the wanted size.
        var minSize = Math.Ceiling(groupSizes / 2m);
        var lastGroupSizeCalc = members.Count() % groupSizes;

        if (lastGroupSizeCalc >= minSize)
        {
          // Should the remainer number (modulo) be greater than the minimum group size, an additional group is needed.
          calculatedChunkCount += 1;
        }
        var groups = new List<string>[calculatedChunkCount];

        Random rng = new Random();
        var iterator = members.OrderBy(a => rng.Next()).GetEnumerator();
        var curGroupI = 0;

        while (iterator.MoveNext())
        {
          if (curGroupI >= groups.Count()) {
            curGroupI = 0;
          }

          var curGroup = groups[curGroupI];
          if (curGroup == null) {
            curGroup = new List<string>();
            groups[curGroupI] = curGroup;
          }
          curGroup.Add(iterator.Current);

          curGroupI++;
        }

        return groups;
      }
    }
  }
}
