using System.Threading;
using System.Threading.Tasks;
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
      public async Task<int> Handle(GenericViewResponseCommand request, CancellationToken cancellationToken)
      {
        switch (request.ViewSubmission.View.CallbackId) {
          case ActionTypes.MemberParticipation: {
            break;
          }
        }

        return 0;
      }
    }
  }
}
