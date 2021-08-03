using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSetting.Commands.UpdateChannelPaused
{
  public class UpdateChannelPauseCommand : IRequest
  {
    public UpdateChannelPauseInput Input { get; set; }

    public class UpdateChannelPauseCommandHandler : CommandBase, IRequestHandler<UpdateChannelPauseCommand>
    {
      private readonly ICurrentUserService currentUserService;

      public UpdateChannelPauseCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) : base(context)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<Unit> Handle(UpdateChannelPauseCommand request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Where(e => e.SlackUserId == currentUserService.UserSlackId && e.ChannelSettingsId == request.Input.ChannelId)
          .FirstOrDefaultAsync(cancellationToken);

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), request.Input);

        channelMember.OnPause = request.Input.Paused;
        channelMember.ReturnFromPauseDate = request.Input.UnPauseDate;
        dbContext.ChannelMembers.Update(channelMember);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}
