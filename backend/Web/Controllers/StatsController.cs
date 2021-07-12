using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Stats.Query.GetMemberStats;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

  public class StatsController : ApiControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<List<StatsDto>>> GetMemberStats([FromQuery] int channelId)
    {
      return await Mediator.Send(new GetMemberStatsQuery{
        ChannelId = channelId
      });
    }
  }
}
