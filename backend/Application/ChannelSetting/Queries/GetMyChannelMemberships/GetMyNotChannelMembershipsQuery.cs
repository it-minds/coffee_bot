using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSetting.Queries.GetMyChannelMemberships
{
  [Authorize]
  public class GetMyNotChannelMembershipsQuery : IRequest<IEnumerable<ChannelMemberDTO>>
  {
    public class GetMyNotChannelMembershipsQueryHandler : IRequestHandler<GetMyNotChannelMembershipsQuery, IEnumerable<ChannelMemberDTO>>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;
      private readonly ICurrentUserService _currentUserService;

      public GetMyNotChannelMembershipsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
      {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
      }

      public async Task<IEnumerable<ChannelMemberDTO>> Handle(GetMyNotChannelMembershipsQuery request, CancellationToken cancellationToken)
      {
        var myChannels = await _context.ChannelSettings
          .Include(x => x.ChannelMembers)
          .Where(x => !x.ChannelMembers.Any(y => y.SlackUserId == _currentUserService.UserSlackId))
          .Select(x => _mapper.Map<ChannelMemberDTO>(x))
          .ToListAsync(cancellationToken);

        return myChannels;
      }
    }
  }
}
