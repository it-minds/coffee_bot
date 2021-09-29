using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelNotices.Commands.CreateChannelNotice
{
  [Authorize]
  public class CreateChannelNoticeCommand : IRequest<int>
  {
    public string Message { get; set; }
    public NoticeType NoticeType { get; set; }
    public int DaysInRound { get; set; }
    public bool Enabled { get; set; }
    public bool Personal { get; set; }
    public int ChannelSettingsId { get; set; }
    public class CreateChannelNoticeCommandHandler : CommandBase, IRequestHandler<CreateChannelNoticeCommand, int>
    {
      public ICurrentUserService currentUserService;
      public CreateChannelNoticeCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<int> Handle(CreateChannelNoticeCommand request, CancellationToken cancellationToken)
      {
        var channelSettings = await dbContext.ChannelSettings
          .FirstOrDefaultAsync(x => x.Id == request.ChannelSettingsId, cancellationToken);

        if (channelSettings == null) throw new NotFoundException(nameof(ChannelSettings), request.ChannelSettingsId);

        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.SlackUserId == currentUserService.UserSlackId)
          .SingleOrDefaultAsync(cancellationToken);

        if (!channelMember.IsAdmin || channelMember.IsRemoved)
        {
          throw new UnauthorizedAccessException("You are not this channel's admin.");
        }

        var otherNoticeExists = await dbContext.ChannelNotices
          .Where(x => x.ChannelSettingsId == request.ChannelSettingsId && x.Personal == request.Personal && x.NoticeType == request.NoticeType)
          .AnyAsync(cancellationToken);

        if (otherNoticeExists)
        {
          // TODO: Exception of some kind
          throw new Exception("Exception of some kind.");
        }

        if (Math.Abs(request.DaysInRound) > channelSettings.DurationInDays )
        {
          // TODO: Exception of some kind
          throw new Exception("Exception of some kind.");
        }

        if (!request.Personal && request.NoticeType == NoticeType.Checkup)
        {
          // TODO: Exception of some kind
          throw new Exception("Exception of some kind.");
        }

        var newNotice = new ChannelNotice {
          Message = request.Message,
          NoticeType = request.NoticeType,
          DaysInRound = request.DaysInRound,
          Enabled = request.Enabled,
          Personal = request.Personal,
          ChannelSettingsId = request.ChannelSettingsId,
        };

        dbContext.ChannelNotices.Add(newNotice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newNotice.Id;
      }
    }
  }
}
