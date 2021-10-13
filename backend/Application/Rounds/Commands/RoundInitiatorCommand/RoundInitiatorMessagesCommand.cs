using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;
using Slack.Messages;

namespace Rounds.Commands.RoundInitiatorCommand
{

  /// <summary>
  ///  This Command
  /// </summary>
  public class RoundInitiatorMessagesCommand
   : IRequest<int>
  {
    public int ChannelSettingsId { get; set; }

    public class RoundInitiatorMessagesCommandHandler : IRequestHandler<RoundInitiatorMessagesCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public RoundInitiatorMessagesCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext) {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(RoundInitiatorMessagesCommand request, CancellationToken cancellationToken)
      {

        var settings = await applicationDbContext.ChannelSettings
          .Where(x => x.Id == request.ChannelSettingsId)
          .FirstOrDefaultAsync();

        var round = await applicationDbContext.CoffeeRounds
          .Where(x => x.Active && x.ChannelId == request.ChannelSettingsId)
          .FirstOrDefaultAsync();

        var groups = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRound.Active && x.CoffeeRoundId == round.Id )
          .ToListAsync();

        await slackClient.SendMessageToChannel(
          conversationId: settings.SlackChannelId,
          text: BuildMessageService.BuildMessage(round.ChannelSettings.RoundStartChannelMessage, round, groups: groups.Select(x => x.CoffeeRoundGroupMembers.Select(y => y.SlackMemberId))),
          cancellationToken: cancellationToken
        );

        if (settings.IndividualMessage)
        {
          var tasks = groups.Select(x => BuildNewCoffeeRoundGroup( round, x, cancellationToken )).ToArray();
          await Task.WhenAll(tasks);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }

      private async Task BuildNewCoffeeRoundGroup(CoffeeRound round, CoffeeRoundGroup group, CancellationToken cancellationToken)
      {
        var members = group.CoffeeRoundGroupMembers.Select(x => x.SlackMemberId);

        var groupMessage = BuildMessageService.BuildMessage(round.ChannelSettings.RoundStartGroupMessage, round);

        var pm = await slackClient.SendPrivateMessageToMembers(cancellationToken, members, groupMessage);
        group.SlackMessageId = pm.ChannelId;

      }
    }
  }
}
