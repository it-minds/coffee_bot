using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;
using Slack.Views;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using Serilog;

namespace Application.Interactivity.Block.EmphPhotoBlockResponse
{
  public class EmphPhotoBlockResponseCommand : IRequest<int>
  {
    public BlockActionRequest BlockActionRequest { get; set; }

    protected string Value { get => ((ButtonAction)BlockActionRequest.Actions[0]).Value; }
    protected string ChannelId { get => BlockActionRequest.Channel.Id; }
    protected string UserId { get => BlockActionRequest.User.Id; }
    protected string ResponseUrl { get => BlockActionRequest.ResponseUrl; }
    protected string TriggerId { get => BlockActionRequest.TriggerId; }

    public class EmphPhotoBlockResponseCommandHandler : IRequestHandler<EmphPhotoBlockResponseCommand, int>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;
      private readonly DownloadImage downloadImage;
      private readonly DeleteSlackMessage deleteSlackMessage;
      private readonly ChannelUserPoints channelUserPoints;
      private readonly WordStrings wordStrings;

      public EmphPhotoBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient, DownloadImage downloadImage, DeleteSlackMessage deleteSlackMessage, ChannelUserPoints channelUserPoints, WordStrings wordStrings)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
        this.downloadImage = downloadImage;
        this.deleteSlackMessage = deleteSlackMessage;
        this.channelUserPoints = channelUserPoints;
        this.wordStrings = wordStrings;
      }

      public async Task<int> Handle(EmphPhotoBlockResponseCommand request, CancellationToken cancellationToken)
      {
        deleteSlackMessage.Enqueue(request.ResponseUrl);

        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRound)
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRound.SlackChannelId == request.ChannelId &&
            x.CoffeeRoundGroupMembers.Any(y => y.SlackMemberId == request.UserId) &&
            x.CoffeeRound.Active)
          .FirstOrDefaultAsync();

        if (group == null) { return 1; }

        var channelMembers = await applicationDbContext.ChannelMembers
          .Where(x =>  x.ChannelSettingsId == group.CoffeeRound.ChannelId)
          .ToListAsync();


        if (request.Value == "Yes")
        {
          foreach (var member in group.CoffeeRoundGroupMembers)
          {
            if (!group.HasMet)
              channelUserPoints.Enqueue(member.SlackMemberId, request.ChannelId);
            if (!group.HasPhoto)
              channelUserPoints.Enqueue(member.SlackMemberId, request.ChannelId, 2);
          }

          var newName = wordStrings.GetPredeterminedStringFromInt(group.Id) + Path.GetExtension(group.SlackPhotoUrl).ToLower();
          downloadImage.Enqueue(group.SlackPhotoUrl, newName);

          group.LocalPhotoUrl = newName;
          if (!group.HasMet) group.FinishedAt = DateTimeOffset.UtcNow;
          group.HasMet = true;
          group.HasPhoto = true;

          var view = MemberParticipationView.Generate(
            group.CoffeeRoundGroupMembers.Select(x => x.SlackMemberId).ToList()
          );

          try
          {
            var result = await slackClient.Client().Views.Open(request.TriggerId, view, cancellationToken);

          }
          catch (SlackException ex)
          {
            foreach (var item in ex.ErrorMessages)
            {
              Log.Error(item);
            }
          }

        } else if (request.Value == "No")
        {
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
