using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Options;

namespace Application.Common
{
  public class DeleteSlackMessage
  {

    private readonly SlackOptions options;

    public DeleteSlackMessage(IOptions<SlackOptions> options)
    {
      this.options = options.Value;
    }

    public void Enqueue(string url)
    {
      BackgroundJob.Enqueue(() => DeleteEmphemetalMessage(url));
    }

    public async Task DeleteEmphemetalMessage(string url)
    {
      var uri = new Uri(url);
      var clientHandler = new HttpClientHandler
      {
          UseCookies = false
      };
      var client = new HttpClient(clientHandler);
      var request = new HttpRequestMessage
      {
          Method = HttpMethod.Post,
          RequestUri = uri,
          Content = new StringContent("{\"delete_original\":\"true\"}", Encoding.UTF8, "application/json"),
          Headers =
          {
            // { "Authorization", "Bearer " + options.BotToken },
          },
      };
      using (var response = await client.SendAsync(request))
      {
          response.EnsureSuccessStatusCode();
      }
    }
  }
}
