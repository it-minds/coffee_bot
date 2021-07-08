using System.Threading.Tasks;
using Application.BlockResponses;
using Microsoft.AspNetCore.Mvc;
using Slack.DTO;

namespace Web.Controllers
{
  public class SlashController : ApiControllerBase
  {
    [HttpPost( "coffee-group-done" )]
    public async Task<ActionResult<BlockResponse>> BlockResponse([FromForm] GenericBlockResponseCommand body)
    {
      var result = await Mediator.Send(body);
      return result;
    }
  }
}
