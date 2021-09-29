using Domain.Enums;

namespace Domain.Entities
{
  public class ChannelNotice
  {
    public int Id { get; set; }
    public string Message { get; set; }
    public NoticeType NoticeType { get; set; }
    public int DaysInRound { get; set; }
    public bool Enabled { get; set; }
    public bool Personal { get; set; }
    public int ChannelSettingsId { get; set; }
    public ChannelSettings ChannelSettings { get; set; }
  }
}
