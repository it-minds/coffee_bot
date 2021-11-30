using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.PredefinedGroups.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PredefinedGroups.Queries.GetChannelsPredefinedGroups
{
  public class GetChannelsPredefinedGroupsQuery : IRequest<IEnumerable<PredefinedGroupDTO>>
  {
    public int ChannelId { get; set; }
    public class GetChannelsPredefinedGroupsQueryHandler : QueryBase, IRequestHandler<GetChannelsPredefinedGroupsQuery, IEnumerable<PredefinedGroupDTO>>
    {
      public GetChannelsPredefinedGroupsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) {}

      public async Task<IEnumerable<PredefinedGroupDTO>> Handle(GetChannelsPredefinedGroupsQuery request, CancellationToken cancellationToken)
      {
        var result = await dbContext.PredefinedGroups
          .Include(x => x.PredefinedGroupMembers)
          .Where(x => x.ChannelSettingsId == request.ChannelId)
          .Select(x => mapper.Map<PredefinedGroupDTO>(x))
          .ToListAsync(cancellationToken);

        return result;
      }
    }
  }
}
