namespace Domain.Entities
{
  public class PredefinedGroupMember
  {
    public int Id { get; set; }
    public PredefinedGroup PredefinedGroup { get; set; }
    public int PredefinedGroupId { get; set; }
    public ChannelMember ChannelMember { get; set; }
    public int ChannelMemberId { get; set; }
  }
}

