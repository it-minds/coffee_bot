using System.Collections.Generic;

namespace Slack.DTO
{
  public class Conversation
  {
    public string Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<string> MemberIds { get; set; }
  }
}
