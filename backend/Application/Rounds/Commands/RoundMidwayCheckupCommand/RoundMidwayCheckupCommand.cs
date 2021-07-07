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
using Slack.Messages;

namespace Rounds.Commands.RoundMidwayCheckupCommand
{

  /// <summary>
  ///  This Command
  /// </summary>
  public class RoundMidwayCheckupCommand: IRequest<int>
  {

    public class RoundMidwayCheckupCommandHandler : IRequestHandler<RoundMidwayCheckupCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public RoundMidwayCheckupCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(RoundMidwayCheckupCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await applicationDbContext.CoffeeRoundGroups
            .Include(x => x.CoffeeRound)
          .Where(x => !x.HasMet && x.CoffeeRound.Active)
          .ToListAsync();

        foreach (var round in activeRounds)
        {
          var daysLeft = (round.CoffeeRound.EndDate - DateTimeOffset.UtcNow).Days;
          var roundDays = (round.CoffeeRound.EndDate - round.CoffeeRound.StartDate).Days;

          if (
            (daysLeft <= roundDays / 2 && round.NotificationCount < 1 ) ||
            (daysLeft <= 1 && round.NotificationCount < 2 )
          ) {
            round.NotificationCount++;

            var message = MidwayMessage.Generate(daysLeft);
            message.Channel = round.SlackMessageId;

            var response = await slackClient.SendMessageToChannel(message: message, cancellationToken: cancellationToken);
          }
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
