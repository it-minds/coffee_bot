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

namespace Application.ChannelSetting.Commands.UpdateChannelSettings
{
  [Authorize]
  public class UpdateChannelSettingsCommand : IRequest
  {
    [JsonIgnore]
    public int Id { get; set; }
    public ChannelSettingsDto Settings { get; set; }

    public class UpdateChannelSettingsCommandHandler : IRequestHandler<UpdateChannelSettingsCommand>
    {
      private readonly IApplicationDbContext dbContext;
      private readonly ICurrentUserService currentUserService;

      public UpdateChannelSettingsCommandHandler(IApplicationDbContext o, ICurrentUserService currentUserService)
      {
        dbContext = o;
        this.currentUserService = currentUserService;
      }

      public async Task<Unit> Handle(UpdateChannelSettingsCommand request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin && x.ChannelSettingsId == request.Id && x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync(cancellationToken);

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        var localSettings = await dbContext.ChannelSettings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken : cancellationToken);
        if (localSettings == null) throw new NotFoundException(nameof(ChannelSettings), request.Id);

        localSettings.GroupSize = request.Settings.GroupSize;
        localSettings.StartsDay = request.Settings.StartsDay;
        localSettings.WeekRepeat = request.Settings.WeekRepeat;
        localSettings.DurationInDays = request.Settings.DurationInDays;
        localSettings.IndividualMessage = request.Settings.IndividualMessage;

        dbContext.ChannelSettings.Update(localSettings);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}
