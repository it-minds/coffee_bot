using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSetting.Queries.GetMyChannelMemberships
{
  [Authorize]
  public class GetMyChannelMembershipQuery : IRequest<ChannelMemberDTO>
  {
    public int ChannelSettingsId { get; set; }
    public class GetMyChannelMembershipQueryHandler : IRequestHandler<GetMyChannelMembershipQuery, ChannelMemberDTO>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;
      private readonly ICurrentUserService _currentUserService;

      public GetMyChannelMembershipQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
      {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
      }

      public async Task<ChannelMemberDTO> Handle(GetMyChannelMembershipQuery request, CancellationToken cancellationToken)
      {
        var myChannel = await _context.ChannelMembers
          .Include(x => x.ChannelSettings)
          .Where(x => x.ChannelSettingsId == request.ChannelSettingsId && x.SlackUserId == _currentUserService.UserSlackId )
          .Select(x => _mapper.Map<ChannelMemberDTO>(x))
          .FirstOrDefaultAsync(cancellationToken);

        return myChannel;
      }
    }
  }
}
