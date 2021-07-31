using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.ChannelSetting;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSetting.Queries.GetChannelSettings
{
  public class GetChannelSettingsQuery : IRequest<ChannelSettingsIdDto>
  {
    public int ChannelSettingsId { get; set; }
    public class GetChannelSettingsQueryHandler : QueryBase, IRequestHandler<GetChannelSettingsQuery, ChannelSettingsIdDto>
    {
      private readonly ICurrentUserService currentUserService;
      public GetChannelSettingsQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<ChannelSettingsIdDto> Handle(GetChannelSettingsQuery request, CancellationToken cancellationToken)
      {
        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.ChannelSettingsId == request.ChannelSettingsId && x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync(cancellationToken);

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        var localSettings = await dbContext.ChannelSettings
          .Select(x => mapper.Map<ChannelSettingsIdDto>(x))
          .FirstOrDefaultAsync(cancellationToken);

        return localSettings;
      }
    }
  }
}
