using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.EntityExtentions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Prizes.Commands.ClaimPrizeForUser
{
  [Authorize]
  public class ClaimPrizeForUserCommand : IRequest<bool>
  {
    // public string SlackUserId { get; set; }
    public int PrizeId { get; set; }

    public class ClaimPrizeForUserCommandHandler : CommandBase, IRequestHandler<ClaimPrizeForUserCommand, bool>
    {
      private readonly ICurrentUserService currentUserService;
      public ClaimPrizeForUserCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
        : base(dbContext)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<bool> Handle(ClaimPrizeForUserCommand request, CancellationToken cancellationToken)
      {
        var prizeToClaim = await dbContext.Prizes.FindAsync(request.PrizeId);

        var member = await dbContext.ChannelMembers
          .Include(x => x.ClaimedPrizes)
            .ThenInclude(x => x.Prize)
          .Where(x => x.SlackUserId == currentUserService.UserSlackId && x.ChannelSettingsId == prizeToClaim.ChannelSettingsId)
          .FirstOrDefaultAsync();

        if (!member.CanAffordPrize(prizeToClaim) || !member.CanClaimPrize(prizeToClaim)) {
          return false;
        }

        var claimedPrize = new ClaimedPrize {
          ChannelMemberId = member.Id,
          PointCost = prizeToClaim.PointCost,
          PrizeId = prizeToClaim.Id,
          DateClaimed = System.DateTimeOffset.UtcNow
        };

        dbContext.ClaimedPrizes.Add(claimedPrize);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
      }
    }
  }
}
