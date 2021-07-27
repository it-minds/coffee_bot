using System.Threading;
using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Interactivity.View.SelectMemeberParticipation;
using MediatR;
using Slack.Views;
using SlackNet.Interaction;

namespace Application.Interactivity.View.GenericViewResponse
{
  public class GenericViewResponseCommand : IRequest<int>
  {
    public ViewSubmission ViewSubmission { get; set; }

    public class GenericViewResponseCommandHandler : IRequestHandler<GenericViewResponseCommand, int>
    {
      private readonly IMediator mediator;

      public GenericViewResponseCommandHandler(IMediator mediator)
      {
        this.mediator = mediator;
      }

      public async Task<int> Handle(GenericViewResponseCommand request, CancellationToken cancellationToken)
      {
        switch (request.ViewSubmission.View.CallbackId) {
          case CallbackIds.MemberParticipation: {
            mediator.Enqueue(new SelectMemeberParticipationCommand {
              ViewSubmission = request.ViewSubmission
            });
            break;
          }
        }

        return 0;
      }
    }
  }
}
