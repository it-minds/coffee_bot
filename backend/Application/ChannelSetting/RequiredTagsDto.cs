using System.Collections.Generic;

namespace Application.ChannelSetting
{
  public class RequiredTagsDto
  {
    public Dictionary<string, string> TagToPredicate { get; set; }
    public IEnumerable<string> StartChannelMessageRequiredTags { get; set; }
    public IEnumerable<string> StartGroupMessageRequiredTags { get; set; }
    public IEnumerable<string> MidwayMessageRequiredTags { get; set; }
    public IEnumerable<string> FinisherMessageRequiredTags { get; set; }

  }
}

