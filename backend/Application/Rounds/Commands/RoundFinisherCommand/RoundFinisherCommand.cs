using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Linq;
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

      private readonly IList<Task> unimportantTasks;

      public RoundFinisherCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
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

        activeRounds.ForEach(round => HandleRound(round, cancellationToken));

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        await Task.WhenAll(unimportantTasks.ToArray());

        return 1;
      }

      private void HandleRound(CoffeeRound round, CancellationToken cancellationToken)
      {
        round.Active = false;
        var meetupPercent = round.CoffeeRoundGroups.Percent(x => x.HasMet);

        var msg = BuildChannelMessage(round, meetupPercent);

        unimportantTasks.Add(slackClient.SendMessageToChannel(cancellationToken, round.ChannelSettings.SlackChannelId, msg));
      }

      private string BuildChannelMessage(CoffeeRound round, decimal meetupPercent)
      {
        var sb = new StringBuilder();

        sb
          .AppendLine("Curtain call ladies and gentlefolk. <!channel>.")
          .AppendLine("Your success has been measured and I give you a solid 10! (For effort.) Your points have been given.")
          .Append("The total meetup rate of the round was: ")
          .Append(Decimal.Round(meetupPercent).ToString())
          .AppendLine("%.");

        if (meetupPercent < 100m) {
          sb.AppendLine("Next time, let's try for 100% shall we?");
        }

        sb.AppendLine("Information regarding your next round TBA. Have a wonderful day :heart:");

        return sb.ToString();
      }
    }
  }
}
