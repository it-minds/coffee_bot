using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Slack.DTO;
using Slack.Interfaces;
using Slack.Views;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;

namespace Application.Interactivity.Block.StatusBlockResponse
{
  public class StatusBlockResponseCommand : IRequest<int>
  {
    public BlockActionRequest BlockActionRequest { get; set; }

    protected string Value { get => ((ButtonAction)BlockActionRequest.Actions[0]).Value; }
    protected string ChannelId { get =>  BlockActionRequest.Channel.Id; }
    protected string TriggerId { get => BlockActionRequest.TriggerId; }

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
        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.SlackMessageId == request.ChannelId)
          .FirstOrDefaultAsync();

        if (group != null) {

          group.HasMet = request.Value == "Yes";
          group.FinishedAt = DateTimeOffset.UtcNow;

          if (group.HasMet)
          {

            var view = MemberParticipationView.Generate(
              group.CoffeeRoundGroupMembers.Select(x => x.SlackMemberId).ToList()
            );

            try
            {
              var result = await slackClient.Client().Views.Open(request.TriggerId, view, cancellationToken);
            }
            catch (SlackException ex)
            {
              foreach (var item in ex.ErrorMessages)
              {
                Log.Error(item);
              }
            }
          }
        }

        await slackClient.UpdateMessage(request.ChannelId, request.BlockActionRequest.Message.Ts, cancellationToken);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
