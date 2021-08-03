using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Prizes.Commands.DeliverClaimedPrize
{
  [Authorize]
  public class DeliverClaimedPrizeCommand : IRequest<int>
  {
    public int ClaimPrizeId { get; set; }
    public class DeliverClaimedPrizeCommandHandler : CommandBase, IRequestHandler<DeliverClaimedPrizeCommand, int>
    {
      private readonly ICurrentUserService currentUserService;
      public DeliverClaimedPrizeCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<int> Handle(DeliverClaimedPrizeCommand request, CancellationToken cancellationToken)
      {
        var claimedPrize = await dbContext.ClaimedPrizes
          .Include(x => x.ChannelMember)
          .Where(x => x.Id == request.ClaimPrizeId)
          .FirstOrDefaultAsync(cancellationToken);

        if (claimedPrize == null) throw new NotFoundException(nameof(ClaimedPrize), request.ClaimPrizeId );

        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin &&
            x.ChannelSettingsId == claimedPrize.ChannelMember.ChannelSettingsId &&
            x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync();

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        claimedPrize.IsDelivered = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
