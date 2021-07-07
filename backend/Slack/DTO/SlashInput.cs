// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slack.DTO
{
  public class User
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("team_id")]
    public string TeamId { get; set; }
  }

  public class Container
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("message_ts")]
    public string MessageTs { get; set; }

    [JsonProperty("channel_id")]
    public string ChannelId { get; set; }

    [JsonProperty("is_ephemeral")]
    public bool IsEphemeral { get; set; }
  }

  public class Team
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("domain")]
    public string Domain { get; set; }
  }

  public class Channel
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
  }

  public class Text
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("text")]
    public string Text2 { get; set; }

    [JsonProperty("verbatim")]
    public bool Verbatim { get; set; }

    [JsonProperty("emoji")]
    public bool Emoji { get; set; }
  }

  public class Element
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("action_id")]
    public string ActionId { get; set; }

    [JsonProperty("text")]
    public Text Text { get; set; }

    [JsonProperty("style")]
    public string Style { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
  }

  public class Block
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("block_id")]
    public string BlockId { get; set; }

    [JsonProperty("text")]
    public Text Text { get; set; }

    [JsonProperty("elements")]
    public List<Element> Elements { get; set; }
  }

  public class Message2
  {
    [JsonProperty("bot_id")]
    public string BotId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("user")]
    public string User { get; set; }

    [JsonProperty("ts")]
    public string Ts { get; set; }

    [JsonProperty("team")]
    public string Team { get; set; }

    [JsonProperty("blocks")]
    public List<Block> Blocks { get; set; }
  }

  public class Values
  {
  }

  public class State
  {
    [JsonProperty("values")]
    public Values Values { get; set; }
  }

  public class Action
  {
    [JsonProperty("action_id")]
    public string ActionId { get; set; }

    [JsonProperty("block_id")]
    public string BlockId { get; set; }

    [JsonProperty("text")]
    public Text Text { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("style")]
    public string Style { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("action_ts")]
    public string ActionTs { get; set; }
  }

  public class SlashInput
  {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }

    [JsonProperty("api_app_id")]
    public string ApiAppId { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("container")]
    public Container Container { get; set; }

    [JsonProperty("trigger_id")]
    public string TriggerId { get; set; }

    [JsonProperty("team")]
    public Team Team { get; set; }

    [JsonProperty("enterprise")]
    public object Enterprise { get; set; }

    [JsonProperty("is_enterprise_install")]
    public bool IsEnterpriseInstall { get; set; }

    [JsonProperty("channel")]
    public Channel Channel { get; set; }

    [JsonProperty("message")]
    public Message2 Message { get; set; }

    [JsonProperty("state")]
    public State State { get; set; }

    [JsonProperty("response_url")]
    public string ResponseUrl { get; set; }

    [JsonProperty("actions")]
    public List<Action> Actions { get; set; }
  }

}