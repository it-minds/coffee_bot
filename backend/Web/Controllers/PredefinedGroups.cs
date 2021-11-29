using System.Threading;
using System.Threading.Tasks;
using Application.CreatePredefinedGroups.Commands.CreatePredefinedGroup;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class PredefinedGroupsController : ApiControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<int>> CreatePredefinedGroup([FromBody] CreatePredefinedGroupCommand body, CancellationToken cancellationToken)
    {
      return await Mediator.Send(body, cancellationToken);
    }
  }
}
