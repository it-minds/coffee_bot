using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.DTO;
using Slack.Interfaces;

namespace Application.User.GetMyAvailableChannels
{
  [Authorize]
  public class GetMyAvailableChannelsQuery : IRequest<IEnumerable<SimpleChannelDTO>>
  {
    public class GetMyAvailableChannelsQueryHandler : QueryBase, IRequestHandler<GetMyAvailableChannelsQuery, IEnumerable<SimpleChannelDTO>>
    {
      private readonly ISlackOAuthClient slackOauthClient;
      private readonly ICurrentUserService currentUserService;

      public GetMyAvailableChannelsQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ISlackOAuthClient slackOauthClient, ICurrentUserService currentUserService)
        : base(dbContext, mapper)
      {
        this.slackOauthClient = slackOauthClient;
        this.currentUserService = currentUserService;
      }

      public async Task<IEnumerable<SimpleChannelDTO>> Handle(GetMyAvailableChannelsQuery request, CancellationToken cancellationToken)
      {
        var token = currentUserService.SlackToken;

        var myChannels = await slackOauthClient.GetUserChannels(token, cancellationToken);

        return myChannels.Select(x => new SimpleChannelDTO
        {
          Id = x.Id,
          Name = x.Name,
          IsPrivate = x.IsPrivate
        });
      }
    }
  }
}
