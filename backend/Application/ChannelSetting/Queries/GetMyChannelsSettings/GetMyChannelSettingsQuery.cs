using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSetting.Queries.GetMyChannelsSettings
{
  /**
   * Not Currently used and might be deleted at a later point
   */
  public class GetMyChannelSettingsQuery : IRequest<List<ChannelSettingsIdDto>>
  {
    public class GetMyChannelSettingsQueryHandler : IRequestHandler<GetMyChannelSettingsQuery, List<ChannelSettingsIdDto>>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;
      private readonly ICurrentUserService _currentUserService;

      public GetMyChannelSettingsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
      {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
      }

      public async Task<List<ChannelSettingsIdDto>> Handle(GetMyChannelSettingsQuery request, CancellationToken cancellationToken)
      {
        var viewModel = new List<ChannelSettingsIdDto>();
        if (_currentUserService.UserSlackId == null) return viewModel;

        var myChannels = await _context.ChannelMembers
          .Where(x => x.SlackUserId == _currentUserService.UserSlackId)
          .Select(x => x.ChannelSettingsId)
          .ToListAsync(cancellationToken);
        viewModel = await _context.ChannelSettings
          .Where(x => myChannels.Contains(x.Id))
          .ProjectTo<ChannelSettingsIdDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);

        return viewModel;
      }
    }
  }
}
