using Newtonsoft.Json;

namespace Slack.DTO
{
  public class BlockResponse
  {
    [JsonProperty("response_type")]
    public string Type { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("replace_original")]
    public bool Replace { get; set; }

    [JsonProperty("delete_original")]
    public bool Delete { get; set; }


  }
}
