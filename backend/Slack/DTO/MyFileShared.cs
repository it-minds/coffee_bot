using System.Collections.Generic;
using Newtonsoft.Json;
using SlackNet.Events;

namespace Slack.DTO
{
  public class MyFileShared: FileShared
  {
    [JsonProperty("channel_id")]
    public string ChannelId { get; set; }
    [JsonProperty("user_id")]
    public string UserId { get; set; }
    [JsonProperty("file_id")]
    public new string FileId { get; set; }
  }
}
