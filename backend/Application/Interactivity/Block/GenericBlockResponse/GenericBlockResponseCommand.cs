using System.Threading;
using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Common.Interfaces;
using Application.Interactivity.Block.EmphPhotoBlockResponse;
using Application.Interactivity.Block.StatusBlockResponse;
using MediatR;
using Slack.Interfaces;
using Slack.Messages;
using SlackNet.Interaction;

namespace Application.Interactivity.Block.GenericBlockResponse
{
  public class GenericBlockResponseCommand : IRequest<int>
  {
    public BlockActionRequest BlockActionRequest { get; set; }

    public class GenericBlockResponseCommandHandler : IRequestHandler<GenericBlockResponseCommand, int>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;
      private readonly IMediator mediator;


      public GenericBlockResponseCommandHandler(IApplicationDbContext applicationDbContext,
        ISlackClient slackClient, IMediator mediator)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
        this.mediator = mediator;

      }

      public async Task<int> Handle(GenericBlockResponseCommand request, CancellationToken cancellationToken)
      {

        switch (request.BlockActionRequest.Action.BlockId) {
          case ActionTypes.MidWay: {

              mediator.Enqueue(new StatusBlockResponseCommand
              {
                BlockActionRequest = request.BlockActionRequest
              });

              break;
          }

          case ActionTypes.EmphemeralPhoto: {

              mediator.Enqueue(new EmphPhotoBlockResponseCommand
              {
                BlockActionRequest = request.BlockActionRequest
              });
              break;
            }

          default: {
            break;
          }
        }
        return 1;
      }
    }
  }
}
