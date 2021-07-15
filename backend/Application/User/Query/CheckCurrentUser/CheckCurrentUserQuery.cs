using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;

namespace User.Query.CheckCurrentUser
{
  [AuthorizeAttribute]
  public class CheckCurrentUserQuery : IRequest<AuthUser>
  {
    public class CheckCurrentUserQueryHandler : IRequestHandler<CheckCurrentUserQuery, AuthUser>
    {
      private readonly ICurrentUserService currentUserService;

      public CheckCurrentUserQueryHandler(ICurrentUserService currentUserService)
      {
        this.currentUserService = currentUserService;
      }

      public async Task<AuthUser> Handle(CheckCurrentUserQuery request, CancellationToken cancellationToken)
      {
        if (currentUserService.UserEmail is null) {
          throw new UnauthorizedAccessException();
        }

        return new AuthUser
        {
          Email = currentUserService.UserEmail,
          SlackUserId = currentUserService.UserSlackId,
          SlackToken = currentUserService.SlackToken
        };
      }
    }
  }
}
