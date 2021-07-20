using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Interfaces;

namespace Application.BlockResponses
{
  public class StatusBlockResponseCommand : IRequest<int>
  {
    public string Payload { get; set; }

    protected SlashInput PayloadParsed { get => JsonConvert.DeserializeObject<SlashInput>(Payload); }

    protected string Value { get => PayloadParsed.Actions[0].Value; }
    protected string ChannelId { get => PayloadParsed.Channel.Id; }

    public class StatusBlockResponseCommandHandler : IRequestHandler<StatusBlockResponseCommand, int>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;
      private readonly ChannelUserPoints channelUserPoints;

      public StatusBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient, ChannelUserPoints channelUserPoints)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
        this.channelUserPoints = channelUserPoints;
      }

      public async Task<int> Handle(StatusBlockResponseCommand request, CancellationToken cancellationToken)
      {
        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.SlackMessageId == request.ChannelId)
          .FirstOrDefaultAsync();

        if (group != null) {

          group.HasMet = request.Value == "Yes";
          group.FinishedAt = DateTimeOffset.UtcNow;

          if (group.HasMet)
          {
            foreach (var member in group.CoffeeRoundGroupMembers)
            {
              channelUserPoints.Enqueue(
                member.SlackMemberId, request.ChannelId
              );
            }
          }
        }

        await slackClient.UpdateMessage(request.ChannelId, request.PayloadParsed.Message.Ts, cancellationToken);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
