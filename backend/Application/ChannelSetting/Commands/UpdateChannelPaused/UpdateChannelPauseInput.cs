namespace Application.ChannelSetting.Commands.UpdateChannelPaused
{
  public class UpdateChannelPauseInput
  {
    public string SlackUserId { get; set; } = "";
    public int ChannelId { get; set; }
    public bool Paused { get; set; }
  }
}
