using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Rounds.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rounds.GetChannelRoundsInRange
{
  public class GetChannelRoundsInRangeQuery : IRequest<IEnumerable<RoundSnipDto>>
  {
    public int ChannelId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public class GetRoundsInRangeQueryHandler : QueryBase, IRequestHandler<GetChannelRoundsInRangeQuery, IEnumerable<RoundSnipDto>>
    {
      public GetRoundsInRangeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) {}

      public async Task<IEnumerable<RoundSnipDto>> Handle(GetChannelRoundsInRangeQuery request, CancellationToken cancellationToken)
      {
        var rounds = await dbContext.CoffeeRounds
          .Include(x => x.CoffeeRoundGroups)
          .Where(x => x.ChannelId == request.ChannelId && request.StartDate <= x.StartDate && request.EndDate >= x.EndDate)
          .ToListAsync();

        return rounds.Select(round => mapper.Map<RoundSnipDto>(round));
      }
    }
  }
}
