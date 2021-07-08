using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Slack.Interfaces;

namespace SlackAuth.Query.GetUserFromCallback
{
  public class GetUserFromCallbackQuery : IRequest<string>
  {
    public string Code { get; set; }

    public class GetUserFromCallbackQueryHandler : IRequestHandler<GetUserFromCallbackQuery, string>
    {
      private readonly ISlackOAuthClient oAuthClient;
      private readonly ITokenService tokenService;
      public GetUserFromCallbackQueryHandler(ISlackOAuthClient oAuthClient, ITokenService tokenService) {
        this.oAuthClient = oAuthClient;
        this.tokenService = tokenService;
      }

      public async Task<string> Handle(GetUserFromCallbackQuery request, CancellationToken cancellationToken)
      {
        var token = await oAuthClient.ExchangeToken(request.Code, CancellationToken.None);

        var slackUser = await oAuthClient.GetUser(token, CancellationToken.None);

        var user = new AuthUser {
          Email = slackUser.Email,
          SlackUserId = slackUser.Id,
          SlackToken = token
        };

        var apptoken = tokenService.CreateToken(user);
        return apptoken;
      }
    }
  }
}
