using System.Collections.Generic;
using System.Threading.Tasks;
using Application.BlockResponses.StatusBlockResponseCommand;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Slack.DTO;

namespace Web.Controllers
{
  public class SlashController : ApiControllerBase
  {
    [HttpPost( "coffee-group-done" )]
    public async Task<ActionResult<bool>> TEST([FromForm] StatusBlockResponseCommand body)
    {
      var result = await Mediator.Send(body);
      return true;
    }
  }
}
