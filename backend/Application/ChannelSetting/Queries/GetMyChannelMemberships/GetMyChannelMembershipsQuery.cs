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
  [AuthorizeAttribute]
  public class GetMyChannelMembershipsQuery : IRequest<IEnumerable<ChannelMemberDTO>>
  {
    public class GetMyChannelMembershipsQueryHandler : IRequestHandler<GetMyChannelMembershipsQuery, IEnumerable<ChannelMemberDTO>>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;
      private readonly ICurrentUserService _currentUserService;

      public GetMyChannelMembershipsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
      {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
      }

      public async Task<IEnumerable<ChannelMemberDTO>> Handle(GetMyChannelMembershipsQuery request, CancellationToken cancellationToken)
      {
        var myChannels = await _context.ChannelMembers
          .Include(x => x.ChannelSettings)
          .Where(x => x.SlackUserId == _currentUserService.UserSlackId)
          .Select(x => _mapper.Map<ChannelMemberDTO>(x))
          .ToListAsync(cancellationToken);

        return myChannels;
      }
    }
  }
}
