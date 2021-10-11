using System;
using System.Collections.Generic;
using System.Linq;
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
   : IRequest<int>
  {

    public class RoundInitiatorCommandHandler : IRequestHandler<RoundInitiatorCommand, int>
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

      public async Task<int> Handle(RoundInitiatorCommand request, CancellationToken cancellationToken)
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

        foreach (var settings in channelSettings)
        {
          if (settings.ChannelMembers.Count() <= 0) {
            continue;
          }
          if (timeService.Now.Hour != settings.InitializeRoundHour) {
            continue;
          }

          var membersToParticipate = settings.ChannelMembers.Where(x => !x.IsRemoved && !x.OnPause).Select(x => x.SlackUserId);
          var groups = SplitChannelIntoSubGroups(membersToParticipate, settings.GroupSize);

          var round = BuildNewCoffeeRound(settings);

          groups.ForEach(group => BuildNewCoffeeRoundGroup(round: round, group: group, cancellationToken: cancellationToken));
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        channelSettings.ForEach(x => mediator.Enqueue(new RoundInitiatorMessagesCommand
        {
          ChannelSettingsId = x.Id
        },
          "New round messages for " + x.SlackChannelName
        ));

        return 1;
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

      private CoffeeRound BuildNewCoffeeRound(ChannelSettings settings)
      {
        var baseDate = DateTimeOffset.UtcNow.Date;
        var daysToAdd = ((int) settings.StartsDay - (int) baseDate.DayOfWeek + 7) % 7;
        var startDate = baseDate.AddDays(daysToAdd);
        var endDate = startDate.AddDays(settings.DurationInDays);

        var round = new CoffeeRound {
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
        var calculatedChunkCount = (int) Math.Floor((decimal) members.Count() / groupSizes);

        // Minimum size of any group is half the wanted size.
        var minSize = Math.Floor(groupSizes / 2m) + 1;
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
