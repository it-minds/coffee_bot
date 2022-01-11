using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Common.Interfaces;
using Application.Common.Linq;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Rounds.Commands.RoundInitiatorCommand
{

  /// <summary>
  ///  This Command
  /// </summary>
  public class RoundInitiatorCommand
   : IRequest<string>
  {

    public class RoundInitiatorCommandHandler : IRequestHandler<RoundInitiatorCommand, string>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IDateTimeOffsetService timeService;
      private readonly IMediator mediator;

      public RoundInitiatorCommandHandler(IApplicationDbContext applicationDbContext, IDateTimeOffsetService timeService, IMediator mediator)
      {
        this.applicationDbContext = applicationDbContext;
        this.mediator = mediator;
        this.timeService = timeService;
      }

      public async Task<string> Handle(RoundInitiatorCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await applicationDbContext.CoffeeRounds
          .Include(x => x.ChannelSettings)
          .Where(x => x.Active)
          .Select(x => x.ChannelSettings.SlackChannelId)
          .ToListAsync();

        var channelSettings = await applicationDbContext.ChannelSettings
          .Include(x => x.ChannelMembers)
          .Where(x => !activeRounds.Contains(x.SlackChannelId))
          .ToListAsync();

        var result = new StringBuilder();

        foreach (var settings in channelSettings)
        {
          if (settings.ChannelMembers.Count() <= 0)
          {
            result.Append("Channel ").Append(settings.SlackChannelName).Append(" currently has no members. Skipped.");
            result.AppendLine();
            continue;
          }
          if (timeService.Now.Hour != settings.InitializeRoundHour)
          {
            result.Append("Channel ").Append(settings.SlackChannelName).Append(" isn't set to start now.");
            result.AppendLine();
            continue;
          }

          var round = BuildNewCoffeeRound(settings);

          var predefinedUsers = await BuildPredefinedGroups(round, cancellationToken);
          result.Append("Channel ").Append(settings.SlackChannelName).Append(" has ").Append(predefinedUsers.Count()).Append(" predefined users.");
          result.AppendLine();

          var membersToParticipate = settings.ChannelMembers
            .Where(x => !x.IsRemoved && !x.OnPause && !predefinedUsers.Contains(x.SlackUserId))
            .Select(x => x.SlackUserId);

          var groups = SplitChannelIntoSubGroups(membersToParticipate, settings.GroupSize);
          result.Append("Channel ").Append(settings.SlackChannelName).Append(" has ").Append(groups.Count()).Append(" groups.");
          result.AppendLine();

          groups.ForEach(group => BuildNewCoffeeRoundGroup(round: round, group: group, cancellationToken: cancellationToken));
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        channelSettings.ForEach(x => mediator.Enqueue(new RoundInitiatorMessagesCommand
        {
          ChannelSettingsId = x.Id
        },
          "New round messages for " + x.SlackChannelName
        ));

        return result.ToString();
      }

      private void BuildNewCoffeeRoundGroup(CoffeeRound round, IEnumerable<string> group, CancellationToken cancellationToken)
      {
        var roundGroup = new CoffeeRoundGroup
        {
          CoffeeRound = round
        };
        applicationDbContext.CoffeeRoundGroups.Add(roundGroup);

        foreach (var memberId in group)
        {
          var member = new CoffeeRoundGroupMember
          {
            SlackMemberId = memberId,
            CoffeeRoundGroup = roundGroup
          };

          applicationDbContext.CoffeeRoundGroupMembers.Add(member);
        }
      }

      private async Task<IEnumerable<string>> BuildPredefinedGroups(CoffeeRound round, CancellationToken cancellationToken)
      {
        var predefinedGroups = await applicationDbContext.PredefinedGroups
          .Include(x => x.PredefinedGroupMembers)
            .ThenInclude(x => x.ChannelMember)
          .Where(x => x.ChannelSettingsId == round.ChannelId)
          .ToListAsync(cancellationToken);

        var result = new List<string>();
        foreach (var predefinedGroup in predefinedGroups)
        {
          var roundGroup = new CoffeeRoundGroup
          {
            CoffeeRound = round
          };

          foreach (var predefinedMember in predefinedGroup.PredefinedGroupMembers)
          {
            var member = new CoffeeRoundGroupMember
            {
              SlackMemberId = predefinedMember.ChannelMember.SlackUserId,
              CoffeeRoundGroup = roundGroup
            };

            applicationDbContext.CoffeeRoundGroupMembers.Add(member);
            applicationDbContext.PredefinedGroupMembers.Remove(predefinedMember);

            result.Add(predefinedMember.ChannelMember.SlackUserId);
          }

          applicationDbContext.CoffeeRoundGroups.Add(roundGroup);
          applicationDbContext.PredefinedGroups.Remove(predefinedGroup);
        }

        return result;
      }

      private CoffeeRound BuildNewCoffeeRound(ChannelSettings settings)
      {
        var baseDate = DateTimeOffset.UtcNow.Date;
        var daysToAdd = ((int)settings.StartsDay - (int)baseDate.DayOfWeek + 7) % 7;
        var startDate = baseDate.AddDays(daysToAdd);
        var endDate = startDate.AddDays(settings.DurationInDays);

        var round = new CoffeeRound
        {
          Active = true,
          ChannelId = settings.Id,
          StartDate = startDate,
          EndDate = endDate
        };

        applicationDbContext.CoffeeRounds.Add(round);

        return round;
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
        var calculatedChunkCount = (int)Math.Floor((decimal)members.Count() / groupSizes);

        // Minimum size of any group is half the wanted size.
        var minSize = Math.Floor(groupSizes / 2m) + 1;
        var lastGroupSizeCalc = members.Count() % groupSizes;

        if (lastGroupSizeCalc >= minSize)
        {
          // Should the remainer number (modulo) be greater than the minimum group size, an additional group is needed.
          calculatedChunkCount += 1;
        }
        var groups = new List<string>[calculatedChunkCount];

        var curGroupI = 0;
        foreach (var member in members.ToList().Shuffle())
        {
          if (curGroupI >= groups.Count())
          {
            curGroupI = 0;
          }

          var curGroup = groups[curGroupI];
          if (curGroup == null)
          {
            curGroup = new List<string>();
            groups[curGroupI] = curGroup;
          }
          curGroup.Add(member);

          curGroupI++;
        }

        return groups;
      }
    }
  }
}
