using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Rounds.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rounds.GetRound
{
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
          .Where(x => x.Id == request.RoundId)
          .ProjectTo<ActiveRoundDto>(mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();

        var members = await applicationDbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == round.ChannelId)
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
