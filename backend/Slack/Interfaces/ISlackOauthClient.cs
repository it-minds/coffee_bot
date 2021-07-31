using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Slack.DTO;

namespace Slack.Interfaces
{
  public interface ISlackOAuthClient
  {

    string BuildRedirectUrl();

    Task<string> ExchangeToken(string code, CancellationToken cancellationToken);
    Task<string> FirstTimeToken(string code, CancellationToken cancellationToken);

    Task<Identity> GetUser(string acessToken,CancellationToken cancellationToken);
    Task<IEnumerable<SlackNet.Conversation>> GetUserChannels(string acessToken, CancellationToken cancellationToken);
  }
}
