using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Stats.Queries.GetMyMatchups;
using Application.Stats.Query.GetMemberStats;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class StatsController : ApiControllerBase
  {
    [HttpGet]
    public async Task<IEnumerable<StatsDto>> GetMemberStats([FromQuery] int channelId, CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMemberStatsQuery { ChannelId = channelId }, cancellationToken);

    [HttpGet("matchups")]
    public async Task<IEnumerable<MatchupDto>> GetMemberMatchups([FromQuery] int channelId, [FromQuery] string slackUserId, CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMyMatchupsQuery { ChannelSettingsId = channelId, SlackUserId = slackUserId }, cancellationToken);

  }
}
