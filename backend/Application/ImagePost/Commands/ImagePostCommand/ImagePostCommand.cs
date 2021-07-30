using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.DTO;
using Slack.Interfaces;
using Slack.Messages;
using SlackNet.Events;

namespace Application.ImagePost.Commands.ImagePostCommand
{
  public class ImagePostCommand : IRequest<int>
  {
    public MyFileShared Event;

    protected string FileId { get => Event.FileId ?? Event.File.Id; }

    public class ImagePostCommandHandler : IRequestHandler<ImagePostCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public ImagePostCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(ImagePostCommand request, CancellationToken cancellationToken)
      {

        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRoundGroupMembers)
          .Include(x => x.CoffeeRound)
            .ThenInclude(x => x.ChannelSettings)
          .Where(x => x.CoffeeRoundGroupMembers.Any(y => y.SlackMemberId == request.Event.UserId))
          .Where(x => x.CoffeeRound.ChannelSettings.SlackChannelId == request.Event.ChannelId && x.CoffeeRound.Active)
          .FirstOrDefaultAsync();

        if (group == null || group.HasPhoto) return 1;

        var file = await slackClient.Client().Files.Info(fileId: request.FileId, cancellationToken: cancellationToken);
        group.SlackPhotoUrl = file.File.UrlPrivate;

        var message = EmphemeralPhotoCheckMessage.Generate();
        message.Channel = request.Event.ChannelId;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        var result = await slackClient.Client().Chat.PostEphemeral(request.Event.UserId, message, cancellationToken);

        return 0;
      }
    }
  }
}
