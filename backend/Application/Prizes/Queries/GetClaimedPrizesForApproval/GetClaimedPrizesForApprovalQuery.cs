using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Prizes.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Prizes.Queries.GetClaimedPrizesForApproval
{
  public class GetClaimedPrizesForApprovalQuery : IRequest<IEnumerable<ClaimedUserPrizeDTO>>
  {
    public int ChannelSettingsId { get; set; }
    public class GetClaimedPrizesForApprovalQueryHandler : QueryBase, IRequestHandler<GetClaimedPrizesForApprovalQuery, IEnumerable<ClaimedUserPrizeDTO>>
    {
      private readonly ICurrentUserService currentUserService;
      public GetClaimedPrizesForApprovalQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<IEnumerable<ClaimedUserPrizeDTO>> Handle(GetClaimedPrizesForApprovalQuery request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin && x.ChannelSettingsId == request.ChannelSettingsId && x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync();

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        var claimedPrizes = await dbContext.ClaimedPrizes
          .Include(x => x.ChannelMember)
          .Include(x => x.Prize)
          .Where(x => !x.IsDelivered && x.ChannelMember.ChannelSettingsId == request.ChannelSettingsId)
          .Select(x => mapper.Map<ClaimedUserPrizeDTO>(x))
          .ToListAsync();

        return claimedPrizes;
      }
    }
  }
}
