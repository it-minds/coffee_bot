namespace Domain.Entities
{
  public class ChannelMember
  {
    public int Id { get; set; }

    public string SlackUserId { get; set; }
    public string SlackName { get; set; }

    public int ChannelSettingsId { get; set; }
    public ChannelSettings ChannelSettings { get; set; }

    public bool OnPause { get; set; } = false;
  }
}
