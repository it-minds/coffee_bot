using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Linq;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;
using Slack.Messages;

namespace Rounds.Commands.RoundFinisherCommand
{

  public class RoundFinisherCommand : IRequest<int>
  {
    public class RoundFinisherCommandHandler : IRequestHandler<RoundFinisherCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IDateTimeOffsetService timeService;
      private readonly IApplicationDbContext applicationDbContext;

      private readonly IList<Task> unimportantTasks;

      public RoundFinisherCommandHandler(ISlackClient slackClient, IDateTimeOffsetService timeService, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
        this.timeService = timeService;
        this.applicationDbContext = applicationDbContext;
        unimportantTasks = new List<Task>();
      }

      public async Task<int> Handle(RoundFinisherCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await applicationDbContext.CoffeeRounds
          .Include(x => x.ChannelSettings)
          .Include(x => x.CoffeeRoundGroups)
            .ThenInclude(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.Active)
          .Where(x => x.EndDate < DateTimeOffset.UtcNow)
          .ToListAsync();

        activeRounds.ForEach(round => {
          if (timeService.Now.Hour != round.ChannelSettings.FinalizeRoundHour) {
            return;
          }
          HandleRound(round, cancellationToken);
          });

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        await Task.WhenAll(unimportantTasks.ToArray());

        return 1;
      }

      private void HandleRound(CoffeeRound round, CancellationToken cancellationToken)
      {
        round.Active = false;
        var meetupPercent = round.CoffeeRoundGroups.Percent(x => x.HasMet);

        var msg = BuildMessageService.BuildMessage(round.ChannelSettings.RoundFinisherMessage, round);

        unimportantTasks.Add(slackClient.SendMessageToChannel(cancellationToken, round.ChannelSettings.SlackChannelId, msg));
      }
    }
  }
}
