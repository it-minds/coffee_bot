using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.User.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace User.Query.CheckCurrentUser
{
  [AuthorizeAttribute]
  public class CheckCurrentUserQuery : IRequest<UserDTO>
  {
    public class CheckCurrentUserQueryHandler : IRequestHandler<CheckCurrentUserQuery, UserDTO>
    {
      private readonly ICurrentUserService currentUserService;
      private readonly IApplicationDbContext dbContext;

      public CheckCurrentUserQueryHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext)
      {
        this.currentUserService = currentUserService;
        this.dbContext = dbContext;
      }

      public async Task<UserDTO> Handle(CheckCurrentUserQuery request, CancellationToken cancellationToken)
      {
        if (currentUserService.UserEmail is null) {
          throw new UnauthorizedAccessException();
        }

        var channelsToAdmin = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin)
          .Select(x => x.ChannelSettingsId)
          .ToListAsync();

        return new UserDTO
        {
          Email = currentUserService.UserEmail,
          SlackUserId = currentUserService.UserSlackId,
          SlackToken = currentUserService.SlackToken,
          ChannelsToAdmin = channelsToAdmin
        };
      }
    }
  }
}
