using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Slack.DTO;
using Slack.Interfaces;
using Slack.Options;
using SlackNet;

namespace Slack.Clients
{
  public class OAuthClient : ISlackOAuthClient
  {
    private readonly ISlackApiClient apiClient;
    private readonly SlackOptions options;

    public OAuthClient(IOptions<SlackOptions> options) {
      this.options = options.Value;

      this.apiClient = new SlackServiceBuilder()
        .UseApiToken(this.options.UserToken)
        .GetApiClient();
    }

    public string BuildRedirectUrl()
    {
      var sb = new StringBuilder("https://slack.com/oauth/authorize");

      sb.Append("?client_id=").Append(options.ClientId);
      sb.Append("&scope=").Append(options.OAuthScope);
      sb.Append("&redirect_url=").Append(options.OAuthRedirectUrl);

      // The state parameter should be used to avoid forgery attacks by passing in a value that's unique to the user you're authenticating and checking it when auth completes.
      sb.Append("&state=").Append("TEST-ABC"); // TODO figure out what state param to use.

      // The team parameters pre specifies the workspace. Omitting this allows the user to select their workspace.
      sb.Append("&team=").Append(options.OAuthTeam);

      return sb.ToString();
    }

    public async Task<string> FirstTimeToken(string code, CancellationToken cancellationToken)
    {
      var result = await apiClient.OAuth.Access(
        clientId: options.ClientId,
        clientSecret: options.ClientSecret,
        code: code,
        redirectUrl: options.OAuthInstallRedirectUrl,
        cancellationToken: cancellationToken
      );

      return result.AccessToken;
    }

    public async Task<string> ExchangeToken(string code, CancellationToken cancellationToken)
    {
      var result = await apiClient.OAuth.Access(
        clientId: options.ClientId,
        clientSecret: options.ClientSecret,
        code: code,
        redirectUrl: options.OAuthRedirectUrl,
        cancellationToken: cancellationToken
      );

      return result.AccessToken;
    }

    public async Task<Identity> GetUser(string acessToken, CancellationToken cancellationToken)
    {
      var result = await apiClient.WithAccessToken(acessToken)
        .Users.Identity(cancellationToken);

      return new Identity {
        Email = result.User.Email,
        Id = result.User.Id
      };
    }
  }
}
