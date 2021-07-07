using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Interfaces;

namespace Application.BlockResponses.StatusBlockResponseCommand
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

      public StatusBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
      }

      public async Task<int> Handle(StatusBlockResponseCommand request, CancellationToken cancellationToken)
      {
        var result = await applicationDbContext.CoffeeRoundGroups
          .Where(x => x.SlackMessageId == request.ChannelId)
          .FirstOrDefaultAsync();

        if (result != null) {
          result.HasMet = request.Value == "Yes";

        }

        await slackClient.UpdateMessage(request.ChannelId, request.PayloadParsed.Message.Ts, cancellationToken);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }

  }
}
