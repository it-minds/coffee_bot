using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.ChannelSetting.Commands.UpdateChannelMessages
{
  [Authorize]
  public class UpdateChannelMessagesCommand : IRequest
  {
    [JsonIgnore]
    public int Id { get; set; }
    public ChannelSettingsDto Settings { get; set; }

    public class UpdateChannelMessagesCommandHandler : IRequestHandler<UpdateChannelMessagesCommand>
    {
      private readonly IApplicationDbContext dbContext;
      private readonly ICurrentUserService currentUserService;

      public UpdateChannelMessagesCommandHandler(IApplicationDbContext o, ICurrentUserService currentUserService)
      {
        dbContext = o;
        this.currentUserService = currentUserService;
      }

      public async Task<Unit> Handle(UpdateChannelMessagesCommand request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin && x.ChannelSettingsId == request.Id && x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync(cancellationToken);

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        var localSettings = await dbContext.ChannelSettings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken : cancellationToken);
        if (localSettings == null) throw new NotFoundException(nameof(ChannelSettings), request.Id);

        localSettings.RoundStartChannelMessage = request.Settings.RoundStartChannelMessage;
        localSettings.RoundStartGroupMessage = request.Settings.RoundStartGroupMessage;
        localSettings.RoundMidwayMessage = request.Settings.RoundMidwayMessage;
        localSettings.RoundFinisherMessage = request.Settings.RoundFinisherMessage;

        dbContext.ChannelSettings.Update(localSettings);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}
