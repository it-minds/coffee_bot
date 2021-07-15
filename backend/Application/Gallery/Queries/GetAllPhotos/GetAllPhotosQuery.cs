using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Gallery.Queries.GetAllPhotos
{
  [AuthorizeAttribute]
  public class GetAllPhotosQuery : IRequest<List<StandardGroupDto>>
  {
    public int ChannelId;

    public class GetAllPhotosQueryHandler : IRequestHandler<GetAllPhotosQuery, List<StandardGroupDto>>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMapper mapper;

      public GetAllPhotosQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
      {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
      }

      public async Task<List<StandardGroupDto>> Handle(GetAllPhotosQuery request, CancellationToken cancellationToken)
      {
        var groups = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRound)
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.HasPhoto && x.CoffeeRound.ChannelId == request.ChannelId && x.PhotoUrl != null && x.PhotoUrl != "")
          .OrderByDescending(x => x.FinishedAt)
          .ProjectTo<StandardGroupDto>(mapper.ConfigurationProvider, 0)
          .ToListAsync(cancellationToken);

        var channelMembers = await applicationDbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == request.ChannelId)
          .ToListAsync();

        foreach (var group in groups)
        {
          group.Members = group.Members.Select(x => channelMembers.FirstOrDefault(y => y.SlackUserId == x)?.SlackName ?? x)
            .ToList();
        }

        return groups;
      }
    }
  }
}
