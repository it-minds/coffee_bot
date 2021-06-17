using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Slack.Interfaces;
using Slack.Options;
using SlackNet;
using SlackNet.WebApi;

namespace Slack.Clients
{
  public class BotClient : ISlackClient
  {

    private readonly ISlackApiClient apiClient;
    private readonly SlackOptions options;

    public BotClient(IOptions<SlackOptions> options)
    {
      this.options = options.Value;

      this.apiClient = new SlackServiceBuilder()
        .UseApiToken(this.options.BotToken)
        .GetApiClient();
    }

    public async Task<string> GetBotId(CancellationToken cancellationToken) {
      var response = await apiClient.Auth.Test(cancellationToken: cancellationToken);

      return response.UserId;
    }

    public async Task<IEnumerable<Slack.DTO.Conversation>> GetAllMyChannels(CancellationToken cancellationToken)
    {
      var channelsImMemberOf = await FetchAllMyChannels(cancellationToken);

      var myId = await GetBotId(cancellationToken);

      var result = new List<Slack.DTO.Conversation>();
      foreach (var channel in channelsImMemberOf)
      {
        var members = await FetchAllChannelMembers(channel.Id, cancellationToken);

        var dto = new Slack.DTO.Conversation
        {
          Id = channel.Id,
          Name = channel.Name,
          MemberIds = members.Where(x => x != myId)
        };
        result.Add(dto);
      }

      return result;
    }

    private async Task<IEnumerable<string>> FetchAllChannelMembers(string channelId, CancellationToken cancellationToken)
    {
      string cursor = "";

      var allMembers = new List<string>();

      do
      {
        var response = await apiClient.Conversations.Members(
          channelId: channelId,
          cancellationToken: cancellationToken,
          limit: 1000,
          cursor: cursor
        );

        allMembers.AddRange(response.Members);

        cursor = response.ResponseMetadata.NextCursor ?? "";

      } while (cursor != "");

      return allMembers;
    }

    private async Task<IEnumerable<Conversation>> FetchAllMyChannels(CancellationToken cancellationToken)
    {
      string cursor = "";

      var channelsImMemberOf = new List<Conversation>();

      do
      {
        var response = await apiClient.Conversations.List(
          cancellationToken: cancellationToken,
          excludeArchived: true,
          limit: 1000,
          types: new List<ConversationType> {
            ConversationType.PublicChannel,
            ConversationType.PrivateChannel
          },
          cursor: cursor
        );

        channelsImMemberOf.AddRange(response.Channels.Where(x => x.IsMember));

        cursor = response.ResponseMetadata.NextCursor ?? "";

      } while (cursor != "" );

      return channelsImMemberOf;
    }

    public async Task<Slack.DTO.SlackThread> SendMessageToChannel(CancellationToken cancellationToken, string conversationId, string text )
    {
      var message = new SlackNet.WebApi.Message
      {
        Text = text,
        Channel = conversationId
      };

      var response = await apiClient.Chat.PostMessage(message: message, cancellationToken:cancellationToken);

      return new Slack.DTO.SlackThread {
        ChannelId = response.Channel,
        MainThreadId = response.Ts,
        IsMainThread = true
      };
    }

    public async Task<Slack.DTO.SlackThread> SendMessageInThread(CancellationToken cancellationToken, Slack.DTO.Conversation conversation, string text, Slack.DTO.SlackThread thread )
    {
      if (!thread.IsMainThread) {
        // TODO error case
        System.Console.WriteLine("MESSAGE ISN'T A MAIN MESSAGE FOR THREAD");
      }

      var message = new SlackNet.WebApi.Message
      {
        Text = text,
        Channel = conversation.Id,
        ThreadTs = thread.MainThreadId
      };

      var response = await apiClient.Chat.PostMessage(message: message, cancellationToken:cancellationToken);

      return new Slack.DTO.SlackThread {
        ChannelId = response.Channel,
        MainThreadId = thread.MainThreadId,
        IsMainThread = false
      };
    }

    public async Task<Slack.DTO.SlackThread> SendPrivateMessageToMembers(CancellationToken cancellationToken, IEnumerable<string> memberIds, string text)
    {
      if (memberIds.Count() <= 1 || memberIds.Count() > 8) {
        return null;
      }

      var mpim = await apiClient.Conversations.Open(memberIds, cancellationToken);

      var result = await SendMessageToChannel(cancellationToken, mpim, text);

      return result;
    }
  }
}
