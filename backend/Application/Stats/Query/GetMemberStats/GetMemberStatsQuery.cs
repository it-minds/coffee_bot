using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stats.Query.GetMemberStats
{
  public class GetMemberStatsQuery: IRequest<List<StatsDto>>
  {
    public int ChannelId { get; set; }

    public class GetMemberStatsQueryHandler : IRequestHandler<GetMemberStatsQuery, List<StatsDto>>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMapper mapper;

      public GetMemberStatsQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
      {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
      }

      public async Task<List<StatsDto>> Handle(GetMemberStatsQuery request, CancellationToken cancellationToken)
      {
        var groups = await applicationDbContext.CoffeeRoundGroupMembers
          .Include(x => x.CoffeeRoundGroup)
            .ThenInclude(x => x.CoffeeRound)
          .Where(x => x.CoffeeRoundGroup.CoffeeRound.ChannelId == request.ChannelId)
          .ProjectTo<MidwayDto>(mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);

        var channelMembers = await applicationDbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == request.ChannelId)
          .ToListAsync();

        var result = new List<StatsDto>();

        foreach (var group in groups.GroupBy(x => x.SlackMemberId))
        {
          var memberId = group.Key;
          var meetupPercent = (group.Count(x => x.HasMet) / (decimal) group.Count()) * 100;
          var photoPercent = meetupPercent > 0m ? (group.Count(x => x.HasPhoto) / (decimal) group.Count(x => x.HasMet)) * 100 : 0m;
          var totalParticipation = group.Count();

          string name = channelMembers.FirstOrDefault(x => x.SlackUserId == group.Key)?.SlackName ?? "";

          var dto = new StatsDto
          {
            SlackMemberId = memberId,
            MeepupPercent = meetupPercent,
            PhotoPercent = photoPercent,
            TotalParticipation = totalParticipation,
            SlackMemberName = name
          };
          result.Add(dto);
        }

        return result;
      }
    }
  }
}
