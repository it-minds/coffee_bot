using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Linq;
using Application.Common.Security;
using Application.Rounds.DTO;
using Application.Rounds.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rounds.GetRound
{
  [AuthorizeAttribute]
  public class GetRoundQuery : IRequest<ActiveRoundDto>
  {
    public int RoundId { get; set; }

    public class GetRoundQueryHandler : IRequestHandler<GetRoundQuery, ActiveRoundDto>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMapper mapper;

      public GetRoundQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
      {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
      }

      public async Task<ActiveRoundDto> Handle(GetRoundQuery request, CancellationToken cancellationToken)
      {
        var round = await applicationDbContext.CoffeeRounds
          .Include(x => x.CoffeeRoundGroups)
            .ThenInclude(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.Id == request.RoundId)
          .ProjectTo<ActiveRoundDto>(mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();

        if (round == null) return null;

        var lastRound = await applicationDbContext.CoffeeRounds
          .Include(x => x.CoffeeRoundGroups)
            .ThenInclude(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.ChannelId == round.ChannelId && !x.Active && x.EndDate < round.StartDate)
          .OrderByDescending(x => x.StartDate)
          .FirstOrDefaultAsync();

        if (lastRound != null) {
          round.PreviousMeetup = lastRound.CoffeeRoundGroups.Percent(x => x.HasMet);
          round.PreviousPhoto = lastRound.CoffeeRoundGroups.Percent(x => x.HasPhoto, x => x.HasMet);
          round.PreviousId = lastRound.Id;
          round.PreviousScore = lastRound.CoffeeRoundGroups.Sum(x => x.GroupScore());
        }

        var nextId = await applicationDbContext.CoffeeRounds
          .Include(x => x.CoffeeRoundGroups)
          .Where(x =>  x.ChannelId == round.ChannelId && x.StartDate > round.EndDate)
          .OrderBy(x => x.StartDate)
          .Select(x => x.Id)
          .FirstOrDefaultAsync();

        if (nextId != default(int)) {
          round.NextId = nextId;
        }

        var members = await applicationDbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == round.ChannelId)
          .ToListAsync();

        foreach (var group in round.CoffeeRoundGroups)
        {
          foreach (var member in group.CoffeeRoundGroupMembers)
          {
            member.SlackMemberName = members.Where(x => x.SlackUserId == member.SlackMemberId).Select(x => x.SlackName).FirstOrDefault();
          }
        }

        return round;
      }
    }
  }
}
