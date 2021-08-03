using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Stats.Query.GetMemberStats;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class StatsController : ApiControllerBase
  {
    [HttpGet]
    public async Task<IEnumerable<StatsDto>> GetMemberStats([FromQuery] int channelId, CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMemberStatsQuery { ChannelId = channelId}, cancellationToken);
  }
}
