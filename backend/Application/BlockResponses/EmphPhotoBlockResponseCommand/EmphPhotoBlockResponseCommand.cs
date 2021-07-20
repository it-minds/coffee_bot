using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Interfaces;
using SlackNet.Blocks;

namespace Application.BlockResponses
{
  public class EmphPhotoBlockResponseCommand : IRequest<int>
  {
    public string Payload { get; set; }

    private SlashInput PayloadParsed { get => JsonConvert.DeserializeObject<SlashInput>(Payload); }

    protected string Value { get => PayloadParsed.Actions[0].Value; }
    protected string ChannelId { get => PayloadParsed.Channel.Id; }
    protected string UserId { get => PayloadParsed.User.Id; }
    protected string ResponseUrl { get => PayloadParsed.ResponseUrl; }

    public class EmphPhotoBlockResponseCommandHandler : IRequestHandler<EmphPhotoBlockResponseCommand, int>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;
      private readonly DownloadImage downloadImage;
      private readonly DeleteSlackMessage deleteSlackMessage;

      public EmphPhotoBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient, DownloadImage downloadImage, DeleteSlackMessage deleteSlackMessage)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
        this.downloadImage = downloadImage;
        this.deleteSlackMessage = deleteSlackMessage;
      }

      public async Task<int> Handle(EmphPhotoBlockResponseCommand request, CancellationToken cancellationToken)
      {
        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRound)
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRound.SlackChannelId == request.ChannelId &&
            x.CoffeeRoundGroupMembers.Any(y => y.SlackMemberId == request.UserId) &&
            x.CoffeeRound.Active)
          .FirstOrDefaultAsync();

        if (group != null) {
          if (request.Value == "Yes")
          {
            group.HasMet = true;
            group.HasPhoto = true;

            var newName = "" + group.Id + Path.GetExtension(group.SlackPhotoUrl).ToLower();

            downloadImage.Enqueue(
              group.SlackPhotoUrl, newName
            );
            group.LocalPhotoUrl = newName;
            group.FinishedAt = DateTimeOffset.UtcNow;

          } else if (request.Value == "No")
          {
          }
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        deleteSlackMessage.Enqueue(request.ResponseUrl);

        return 1;
      }
    }
  }
}
