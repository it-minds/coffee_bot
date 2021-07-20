using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;

namespace Application.ChannelSync.Commands
{
  public class SyncronizeChannelsCommand : IRequest<int>
  {

    public class SyncronizeChannelsCommandHandler : IRequestHandler<SyncronizeChannelsCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMediator mediator;

      public SyncronizeChannelsCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext, IMediator mediator) {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
        this.mediator = mediator;
      }

      public async Task<int> Handle(SyncronizeChannelsCommand request, CancellationToken cancellationToken)
      {

        var channelsImMemberOf = await slackClient.GetAllMyChannels(cancellationToken);

        var currentChannels = await applicationDbContext.ChannelSettings.ToListAsync();

        var (removedChannels, updatedChannels, newChannels) = SegmentJoin(
          currentChannels,
          channelsImMemberOf,
          x => x.SlackChannelId,
          x => x.Id
        );

        SyncRemovedChannels(removedChannels);
        SyncAddedChannels(newChannels);
        await SyncUpdatedChannels(updatedChannels, cancellationToken);

        var result = await applicationDbContext.SaveChangesAsync(cancellationToken);

        return result;
      }

      private async Task SyncUpdatedChannels(IEnumerable<(ChannelSettings, Slack.DTO.Conversation)> updatedChannels, CancellationToken cancellationToken)
      {
        var existingMembers = await applicationDbContext.ChannelMembers.ToListAsync();

        foreach (var (settings, slackConvo) in updatedChannels)
        {
          settings.SlackChannelName = slackConvo.Name;

          var (removedMembers, updatedMembers, newMembers) = SegmentJoin(
            existingMembers.Where(x => x.ChannelSettingsId == settings.Id),
            slackConvo.MemberIds,
            x => x.SlackUserId,
            x => x
          );
          // applicationDbContext.ChannelMembers.RemoveRange(removedMembers);
          foreach (var removedMember in removedMembers)
          {
            removedMember.IsRemoved = true;
            removedMember.SlackName = "";
          }

          var members = updatedMembers.Select(x => x.Item1).ToList();
          members.AddRange(newMembers.Select(x => new ChannelMember
          {
            SlackUserId = x,
            ChannelSettingsId = settings.Id
          }));

          var slackInfoMembers = await slackClient.GetUsers(members.Select(x => x.SlackUserId), cancellationToken);

          foreach (var item in members)
          {
            var slackInfo = slackInfoMembers.FirstOrDefault(x => x.Id == item.SlackUserId);

            if (slackInfo != null)
            {
              item.SlackName = slackInfo.Profile.RealName;
            }

            if (item.Id > 0) {
              applicationDbContext.ChannelMembers.Update(item);
            } else {
              applicationDbContext.ChannelMembers.Add(item);
            }
          }
        }
      }

      private void SyncRemovedChannels(IEnumerable<ChannelSettings> removedChannels)
      {
        applicationDbContext.ChannelSettings.RemoveRange(removedChannels);
      }

      private void SyncAddedChannels(IEnumerable<Slack.DTO.Conversation> addedChannels)
      {
        foreach (var newChannel in addedChannels)
        {
          var entitiy = applicationDbContext.ChannelSettings.Add(new ChannelSettings
          {
            SlackChannelId = newChannel.Id,
            SlackChannelName = newChannel.Name
          }).Entity;

          applicationDbContext.ChannelMembers.AddRange(newChannel.MemberIds.Select(x => new ChannelMember {
            SlackUserId = x,
            ChannelSettings = entitiy
          }));

          mediator.Enqueue(new NewChannelMessagerCommand {
            SlackChannelId = newChannel.Id
          });
        }
      }


      private (IEnumerable<TLeft>, IEnumerable<(TLeft, TRight)>, IEnumerable<TRight>) SegmentJoin<TLeft, TRight, TKey> (
        IEnumerable<TLeft> leftItems,
        IEnumerable<TRight> rightItems,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector
      )
        where TKey : IEquatable<TKey>
      {
        var segments = SegmentOuterJoin(leftItems: leftItems, rightItems: rightItems, leftKeySelector: leftKeySelector, rightKeySelector: rightKeySelector);

        var onlyLeftItems = segments.Where(x => x.Item2 is null).Select(x => x.Item1);
        var onlyRightItems = segments.Where(x => x.Item1 is null).Select(x => x.Item2);
        var innerItems = segments.Where(x => !(x.Item1 is null) && !(x.Item2 is null));

        var result = (
          onlyLeftItems,
          innerItems,
          onlyRightItems
        );

        return result;
      }

      // ! Should have a runtime of O(n + m)
      private IEnumerable<(TLeft, TRight)> SegmentOuterJoin<TLeft, TRight, TKey>(
        IEnumerable<TLeft> leftItems,
        IEnumerable<TRight> rightItems,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector
      )
        where TKey : IEquatable<TKey>
      {
        var dict = new Dictionary<TKey, (TLeft, TRight)>();

        foreach (var left in leftItems)
        {
          dict.Add(leftKeySelector(left), (left, default(TRight)));
        }

        foreach (var right in rightItems)
        {
          var key = rightKeySelector(right);

          if (dict.ContainsKey(key)) {
            dict[key] = (dict[key].Item1, right);
          } else {
            dict.Add(key, (default(TLeft), right));
          }
        }

        return dict.Values;
      }
    }
  }
}
