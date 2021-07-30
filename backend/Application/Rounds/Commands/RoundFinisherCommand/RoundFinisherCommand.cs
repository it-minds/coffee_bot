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
      private readonly ChannelUserPoints channelUserPoints;

      public RoundFinisherCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext,
        ChannelUserPoints channelUserPoints)
      {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
        this.channelUserPoints = channelUserPoints;
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

        IList<Task> sendMessageTasks = new List<Task>();

        activeRounds.ForEach(round => HandleRound(round, ref sendMessageTasks, cancellationToken));

        Task.WaitAll(sendMessageTasks.ToArray());

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }

      private void HandleRound(CoffeeRound round, ref IList<Task> sendMessageTasks, CancellationToken cancellationToken)
      {
        round.Active = false;
        var meetupPercent = round.CoffeeRoundGroups.Percent(x => x.HasMet);

        foreach (var group in round.CoffeeRoundGroups.Where(x => x.HasMet))
        {
          channelUserPoints.Enqueue(
            group.CoffeeRoundGroupMembers.Where(x => x.Participated).Select(x => x.SlackMemberId),
            round.ChannelId,
            group.HasPhoto ? 2 : 1
          );
        }

        var msg = BuildChannelMessage(round, meetupPercent);

        sendMessageTasks.Add(slackClient.SendMessageToChannel(cancellationToken, round.ChannelSettings.SlackChannelId, msg));
      }

      private string BuildChannelMessage(CoffeeRound round, decimal meetupPercent)
      {
        var sb = new StringBuilder();

        sb
          .AppendLine("Curtain call ladies and gentlefolk. <!channel>.")
          .AppendLine("Your success has been measured and I give you a solid 10! (For effort.) Your points has been given.")
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
