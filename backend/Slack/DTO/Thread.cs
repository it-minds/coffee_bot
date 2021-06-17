namespace Slack.DTO
{
  public class SlackThread
  {
    public bool IsMainThread { get; set; }
    public string ChannelId { get; set; }
    public string MainThreadId { get; set; }
  }
}
