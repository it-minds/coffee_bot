using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Slack.Interfaces;

namespace SlackAuth.Query.GetAuthUrl
{
  public class GetAuthUrlQuery : IRequest<string>
  {

    public class GetAuthUrlQueryHandler : IRequestHandler<GetAuthUrlQuery, string>
    {

      private readonly ISlackOAuthClient oAuthClient;
      public GetAuthUrlQueryHandler(ISlackOAuthClient oAuthClient)
      {
        this.oAuthClient = oAuthClient;
      }

#pragma warning disable 1998
      public async Task<string> Handle(GetAuthUrlQuery request, CancellationToken cancellationToken)
      {
        var url = oAuthClient.BuildRedirectUrl();
        return url;
      }
#pragma warning restore 1998
    }
  }
}
