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
    public async Task<IEnumerable<StatsDto>> GetMemberStats([FromQuery] GetMemberStatsQuery request, CancellationToken cancellationToken) =>
      await Mediator.Send(request, cancellationToken);
  }
}
