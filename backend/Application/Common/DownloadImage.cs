using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Options;
using Slack.Options;

namespace Application.Common
{
  public class DownloadImage
  {

    private readonly SlackOptions options;

    public DownloadImage(IOptions<SlackOptions> options)
    {
      this.options = options.Value;
    }

    public void Enqueue(string filePath, string newName)
    {
      BackgroundJob.Enqueue(() => DownloadAndSaveImage(filePath, newName));
    }

    public async Task DownloadAndSaveImage(string filePath, string newName)
    {
      var uri = new Uri(filePath);
      var clientHandler = new HttpClientHandler
      {
          UseCookies = false
      };
      var client = new HttpClient(clientHandler);
      var request = new HttpRequestMessage
      {
          Method = HttpMethod.Get,
          RequestUri = uri,
          Headers =
          {
              { "Authorization", "Bearer " + options.BotToken },
          },
      };
      using (var response = await client.SendAsync(request))
      {
          response.EnsureSuccessStatusCode();
          var body = await response.Content.ReadAsByteArrayAsync();
          await File.WriteAllBytesAsync("wwwroot/images/coffeegroups/"+ newName , body);
      }
    }
  }
}
