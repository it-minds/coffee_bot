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

namespace Rounds.Commands.RoundFinisherCommand
{

  public class RoundFinisherCommand : IRequest<int>
  {
    public class RoundFinisherCommandHandler : IRequestHandler<RoundFinisherCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public RoundFinisherCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(RoundFinisherCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await applicationDbContext.CoffeeRounds
          .Include(x => x.ChannelSettings)
          .Include(x => x.CoffeeRoundGroups)
          .Where(x => x.Active)
          .Where(x => x.EndDate < DateTimeOffset.UtcNow)
          .ToListAsync();

        foreach (var round in activeRounds)
        {
          round.Active = false;

          var meetupPercent = round.CoffeeRoundGroups.Where(x => x.HasMet).Count() / round.CoffeeRoundGroups.Count() * 100;

          var msg = BuildChannelMessage(round, meetupPercent);

          await slackClient.SendMessageToChannel(cancellationToken, round.SlackChannelId, msg);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }

      private string BuildChannelMessage(CoffeeRound round, decimal meetupPercent)
      {
        var sb = new StringBuilder();

        sb
          .AppendLine("Curtain call ladies and gentlefolk. <!channel>.")
          .AppendLine("Your success has been measured and I give you a solid 10! (for effort)")
          .Append("Your total meetup percent was: ")
          .Append(Decimal.Round(meetupPercent).ToString())
          .AppendLine("%");

        if (meetupPercent < 100m) {
          sb.AppendLine("Next time, let's try for 100% shall we?");
        }

        sb.AppendLine("Information regarding your next round TBA. Have a wonderful day :heart:");

        return sb.ToString();
      }
    }
  }
}
