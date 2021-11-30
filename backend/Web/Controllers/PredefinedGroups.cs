using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CreatePredefinedGroups.Commands.CreatePredefinedGroup;
using Application.PredefinedGroups.DTOs;
using Application.PredefinedGroups.Queries.GetChannelsPredefinedGroups;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class PredefinedGroupsController : ApiControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<int>> CreatePredefinedGroup([FromBody] CreatePredefinedGroupCommand body, CancellationToken cancellationToken) => await Mediator.Send(body, cancellationToken);

    [HttpGet]
    public async Task<IEnumerable<PredefinedGroupDTO>> GetChannelsPredefinedGroups([FromQuery] int channelId, CancellationToken cancellationToken) => await Mediator.Send(new GetChannelsPredefinedGroupsQuery {
      ChannelId = channelId
    }, cancellationToken);
  }
}
