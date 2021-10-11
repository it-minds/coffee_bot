using System.Collections.Generic;

namespace Application.ChannelSetting
{
  public class RequiredTagsDto
  {
    public Dictionary<string, string> TagToPredicate { get; set; }
    public List<string> StartChannelMessageRequiredTags { get; set; }
    public List<string> StartGroupMessageRequiredTags { get; set; }
    public List<string> MidwayMessageRequiredTags { get; set; }
    public List<string> FinisherMessageRequiredTags { get; set; }

  }
}

