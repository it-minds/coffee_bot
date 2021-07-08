using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Interfaces;
using Slack.Messages;

namespace Application.BlockResponses
{
  public class GenericBlockResponseCommand : IRequest<BlockResponse>
  {
    public string Payload { get; set; }

    protected SlashInput PayloadParsed { get => JsonConvert.DeserializeObject<SlashInput>(Payload); }

    protected string ActionId {get => PayloadParsed.Actions[0].BlockId; }


    public class GenericBlockResponseCommandHandler : IRequestHandler<GenericBlockResponseCommand, BlockResponse>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;

      private readonly IMediator mediator;

      public GenericBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient, IMediator mediator)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
        this.mediator = mediator;
      }

      public async Task<BlockResponse> Handle(GenericBlockResponseCommand request, CancellationToken cancellationToken)
      {
        switch (request.ActionId) {
          case ActionTypes.MidWay: {

              mediator.Enqueue(new StatusBlockResponseCommand
              {
                Payload = request.Payload
              });

              break;
          }

          case ActionTypes.EmphemeralPhoto: {

              var result = await mediator.Send(new EmphPhotoBlockResponseCommand
              {
                Payload = request.Payload
              });

              return result;
          }

          default: {
            break;
          }
        }


        return null;
      }
    }
  }
}
