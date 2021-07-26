namespace Application.Prizes.Queries.GetChannelPrizes
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Application.Common.Interfaces;
  using Application.Prizes.Common;
  using AutoMapper;
  using MediatR;
  using Microsoft.EntityFrameworkCore;

  public class GetChannelPrizesQuery : IRequest<IEnumerable<PrizeIdDTO>>
  {
    public int ChannelSettingsId { get; set; }
    public class GetChannelPrizesQueryHandler : IRequestHandler<GetChannelPrizesQuery, IEnumerable<PrizeIdDTO>>
    {
      private readonly IApplicationDbContext dbContext;
      private readonly IMapper autoMapper;
      private readonly ICurrentUserService currentUserService;

      public GetChannelPrizesQueryHandler(IApplicationDbContext dbContext, IMapper autoMapper, ICurrentUserService currentUserService)
      {
        this.dbContext = dbContext;
        this.autoMapper = autoMapper;
        this.currentUserService = currentUserService;
      }

      public async Task<IEnumerable<PrizeIdDTO>> Handle(GetChannelPrizesQuery request, CancellationToken cancellationToken)
      {
        var prizes = await dbContext.Prizes
          .Where(x => x.ChannelSettingsId == request.ChannelSettingsId)
          .Select(x => autoMapper.Map<PrizeIdDTO>(x))
          .ToListAsync();

        return prizes;
      }
    }
  }
}
