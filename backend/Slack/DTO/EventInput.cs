using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slack.DTO
{
  // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class File
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("file_id")]
        public string FileId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("file")]
        public File File { get; set; }

        [JsonProperty("event_ts")]
        public string EventTs { get; set; }
    }

    public class Authorization
    {
        [JsonProperty("enterprise_id")]
        public object EnterpriseId { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("is_enterprise_install")]
        public bool IsEnterpriseInstall { get; set; }
    }

    public class EventInput
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("api_app_id")]
        public string ApiAppId { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("event_time")]
        public int EventTime { get; set; }

        [JsonProperty("authorizations")]
        public List<Authorization> Authorizations { get; set; }

        [JsonProperty("is_ext_shared_channel")]
        public bool IsExtSharedChannel { get; set; }

        [JsonProperty("event_context")]
        public string EventContext { get; set; }
    }


}
