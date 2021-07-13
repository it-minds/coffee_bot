using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rounds.GetCurrentRound
{
  public class GetCurrentRoundQuery : IRequest<ActiveRoundDto>
  {
    public int ChannelId { get; set; }

    public class GetCurrentRoundQueryHandler : IRequestHandler<GetCurrentRoundQuery, ActiveRoundDto>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMapper mapper;

      public GetCurrentRoundQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
      {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
      }

      public async Task<ActiveRoundDto> Handle(GetCurrentRoundQuery request, CancellationToken cancellationToken)
      {
        var round = await applicationDbContext.CoffeeRounds
          .Where(x => x.Active && x.ChannelId == request.ChannelId)
          .ProjectTo<ActiveRoundDto>(mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();

        var lastRound = await applicationDbContext.CoffeeRounds
          .Include(x => x.CoffeeRoundGroups)
          .Where(x => !x.Active && x.ChannelId == request.ChannelId)
          .OrderByDescending( x=> x.EndDate)
          .FirstOrDefaultAsync();

        if (lastRound != null) {
          round.PreviousMeetup = (decimal)lastRound.CoffeeRoundGroups.Count(x => x.HasMet) / lastRound.CoffeeRoundGroups.Count();
          round.PreviousPhoto = round.PreviousMeetup > 0 ? (decimal) lastRound.CoffeeRoundGroups.Count(x => x.HasPhoto) / lastRound.CoffeeRoundGroups.Count( x => x.HasMet) : 0m ;
        }

        var members = await applicationDbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == request.ChannelId)
          .ToListAsync();

        foreach (var group in round.Groups)
        {
          group.Members = members.Where(x => group.Members.Contains(x.SlackUserId)).Select(x => x.SlackName).ToList() ;
        }

        return round;
      }
    }
  }
}
