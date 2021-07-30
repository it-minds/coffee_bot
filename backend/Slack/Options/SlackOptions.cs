namespace Slack.Options
{
  public class SlackOptions
  {
    public  const string SlackOptionsToken = "SlackOptions";
    public string BotToken { get; set; }
    public string UserToken { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public string OAuthScope { get; set; }
    public string OAuthTeam { get; set; }
    public string OAuthRedirectUrl { get; set; }
    public string OAuthInstallRedirectUrl { get; set; }
  }
}
