using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.EntityExtentions;
using Application.Common.Interfaces;
using Application.Prizes.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Prizes.Queries.GetUserPrizes
{
  public class GetUserPrizesQuery : IRequest<UserPrizesDTO>
  {
    public string SlackUserId { get; set; }
    public int ChannelId { get; set; }

    public class GetUserPrizesQueryHandler : QueryBase, IRequestHandler<GetUserPrizesQuery, UserPrizesDTO>
    {
      public GetUserPrizesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) {}

      public async Task<UserPrizesDTO> Handle(GetUserPrizesQuery request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Include(x => x.ClaimedPrizes)
          .Where(x => x.SlackUserId == request.SlackUserId && x.ChannelSettingsId == request.ChannelId)
          .FirstOrDefaultAsync();

        var prizesClaimed = await dbContext.ClaimedPrizes
          .Include(x => x.Prize)
          .Where(x => x.ChannelMemberId == channelMember.Id)
          .OrderByDescending(x => x.DateClaimed)
          .Select(x => mapper.Map<ClaimedPrizeDTO>(x))
          .ToListAsync();

        var prizes = await dbContext.Prizes
          .Where(x => x.ChannelSettingsId == request.ChannelId)
          .ToListAsync();

        var result = new UserPrizesDTO
        {
          SlackUserId = channelMember.SlackUserId,
          Points = channelMember.Points,
          PointsRemaining = channelMember.PointsRemaining(),

          PrizesClaimed = prizesClaimed,
          PrizesAvailable = prizes.Where(x => channelMember.CanClaimAndAffordPrize(x))
            .Select(x => mapper.Map<PrizeIdDTO>(x))
        };

        return result;
      }
    }
  }
}
