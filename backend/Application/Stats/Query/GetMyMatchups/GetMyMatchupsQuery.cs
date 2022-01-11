using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stats.Queries.GetMyMatchups
{
  [Authorize]
  public class GetMyMatchupsQuery : IRequest<IEnumerable<MatchupDto>>
  {
    public int ChannelSettingsId { get; set; }
    public string SlackUserId { get; set; }

    public class GetMyMatchupsQueryHandler : QueryBase, IRequestHandler<GetMyMatchupsQuery, IEnumerable<MatchupDto>>
    {
      private readonly ICurrentUserService currentUserService;

      public GetMyMatchupsQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<IEnumerable<MatchupDto>> Handle(GetMyMatchupsQuery request, CancellationToken cancellationToken)
      {
        var slackMemberId = await dbContext.ChannelMembers
          .Where(x => x.SlackUserId == (string.IsNullOrEmpty(request.SlackUserId) ? currentUserService.UserSlackId : request.SlackUserId) && x.ChannelSettingsId == request.ChannelSettingsId)
          .Select(x => x.SlackUserId)
          .FirstOrDefaultAsync(cancellationToken);

        if (slackMemberId == default(string)) throw new NotFoundException();

        var allMyGroupMembers = await dbContext.CoffeeRoundGroups.Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRoundGroupMembers.Any(y => y.SlackMemberId == slackMemberId))
          .SelectMany(x => x.CoffeeRoundGroupMembers)
          .ToListAsync(cancellationToken);

        var score = new Dictionary<string, int>();

        foreach (var member in allMyGroupMembers)
        {
          if (member.SlackMemberId == slackMemberId) continue;

          score[member.SlackMemberId] = score.GetValueOrDefault(member.SlackMemberId) + 1;
        }

        var result = score.Select(x => new MatchupDto
        {
          Name = x.Key,
          Count = x.Value
        });

        return result;
      }
    }
  }
}