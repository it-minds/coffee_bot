using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Prizes.Commands.ClaimPrizeForUser;
using Application.Prizes.Commands.CreateChannelPrize;
using Application.Prizes.Common;
using Application.Prizes.Queries.GetChannelPrizes;
using Application.Prizes.Queries.GetClaimedPrizesForApproval;
using Application.Prizes.Queries.GetUserPrizes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class PrizesController : ApiControllerBase
  {
    private readonly ICurrentUserService currentUserService;

    public PrizesController(ICurrentUserService currentUserService)
    {
      this.currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IEnumerable<PrizeIdDTO>> GetChannelPrizes([FromQuery] int channelId, CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetChannelPrizesQuery {
        ChannelSettingsId = channelId
      }, cancellationToken);
    }

    [HttpPost]
    public async Task<int> CreateChannelPrize([FromBody] CreateChannelPrizeCommand body , CancellationToken cancellationToken)
    {
      return await Mediator.Send(body, cancellationToken);
    }

    [HttpGet("user/{slackUserId}")]
    public async Task<UserPrizesDTO> GetUserPrizes([FromRoute] string slackUserId, [FromQuery] int channelId, CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetUserPrizesQuery {
        ChannelId = channelId,
        SlackUserId = slackUserId
      }, cancellationToken);
    }

    [HttpGet("user/mine")]
    [Authorize]
    public async Task<UserPrizesDTO> GetMyPrizes([FromQuery] int channelId, CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetUserPrizesQuery {
        ChannelId = channelId,
        SlackUserId = currentUserService.UserSlackId,
      }, cancellationToken);
    }

    [HttpPost("claim")]
    public async Task<bool> ClaimPrizeForUser([FromBody] ClaimPrizeForUserCommand body , CancellationToken cancellationToken)
    {
      return await Mediator.Send(body, cancellationToken);
    }

    [HttpGet("admin/claim")]
    public async Task<IEnumerable<ClaimedUserPrizeDTO>> GetClaimedPrizesForApproval(
      [FromQuery] GetClaimedPrizesForApprovalQuery input,
      CancellationToken cancellationToken) =>
    await Mediator.Send(input, cancellationToken);
  }
}
