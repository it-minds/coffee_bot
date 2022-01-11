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
  public class RoundInitiatorMessagesCommand : IRequest<string>
  {
    public int ChannelSettingsId { get; set; }

    public class RoundInitiatorMessagesCommandHandler : IRequestHandler<RoundInitiatorMessagesCommand, string>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;
      public RoundInitiatorMessagesCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<string> Handle(RoundInitiatorMessagesCommand request, CancellationToken cancellationToken)
      {
        var settings = await applicationDbContext.ChannelSettings
          .Where(x => x.Id == request.ChannelSettingsId)
          .FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
        {
          return "No settings";
        }

        var round = await applicationDbContext.CoffeeRounds
          .Where(x => x.Active && x.ChannelId == request.ChannelSettingsId)
          .FirstOrDefaultAsync(cancellationToken);

        if (round == null)
        {
          return "No round";
        }

        var groups = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRoundId == round.Id)
          .ToListAsync(cancellationToken);

        await slackClient.SendMessageToChannel(
          conversationId: settings.SlackChannelId,
          text: BuildChannelMessage(round, groups.Select(x => x.CoffeeRoundGroupMembers.Select(y => y.SlackMemberId))),
          cancellationToken: cancellationToken
        );

        if (settings.IndividualMessage)
        {
          var tasks = groups.Select(x => BuildNewCoffeeRoundGroup(round, x, cancellationToken)).ToArray();
          await Task.WhenAll(tasks);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return "Success";
      }

      private async Task BuildNewCoffeeRoundGroup(CoffeeRound round, CoffeeRoundGroup group, CancellationToken cancellationToken)
      {
        var members = group.CoffeeRoundGroupMembers.Select(x => x.SlackMemberId);

        var groupMessage = BuildGroupMessage(round, members);

        var pm = await slackClient.SendPrivateMessageToMembers(cancellationToken, members, groupMessage);
        group.SlackMessageId = pm.ChannelId;

      }

      private string BuildGroupMessage(CoffeeRound round, IEnumerable<string> group)
      {
        var sb = new StringBuilder();
        sb.AppendLine("Time for your coffee!");

        sb.Append("The round starts: ")
          .Append(round.StartDate.ToString("dddd, dd/MMMM"))
          .Append(". The round ends: ")
          .Append(round.EndDate.ToString("dddd, dd/MMMM"))
          .AppendLine(".")
          .AppendLine("Have fun!");


        return sb.ToString();
      }

      private string BuildChannelMessage(CoffeeRound round, IEnumerable<IEnumerable<string>> groups)
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

          sb.Append("Group " + (i + 1))
            .Append(": ")
            .AppendJoin(", ", group.Select(x => "<@" + x + ">"))
            .AppendLine("");
        }

        return sb.ToString();
      }
    }
  }
}
