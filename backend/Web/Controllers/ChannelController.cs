using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.ChannelSetting;
using Application.ChannelSetting.Commands.UpdateChannelPaused;
using Application.ChannelSetting.Commands.UpdateChannelSettings;
using Application.ChannelSetting.Queries.GetChannelSettings;
using Application.ChannelSetting.Queries.GetMyChannelMemberships;
using Application.Rounds.DTO;
using Application.Rounds.GetChannelRounds;
using Application.Rounds.GetCurrentRound;
using Application.Rounds.GetChannelRoundsInRange;
using Application.User.GetMyAvailableChannels;
using Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
  public class ChannelController : ApiControllerBase
  {
    [HttpGet("MyChannelMemberships")]
    public async Task<IEnumerable<ChannelMemberDTO>> GetMyChannelMemberships(CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMyChannelMembershipsQuery {}, cancellationToken);

    [HttpGet("MyChannelMemberships/{channelSettingsId}")]
    public async Task<ChannelMemberDTO> GetMyChannelMembership([FromRoute] int channelSettingsId, CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMyChannelMembershipQuery {
        ChannelSettingsId = channelSettingsId
      }, cancellationToken);

    [HttpGet("MyChannelMemberships/not")]
    public async Task<IEnumerable<ChannelMemberDTO>> GetMyNotChannelMemberships(CancellationToken cancellationToken) =>
      await Mediator.Send(new GetMyNotChannelMembershipsQuery {}, cancellationToken);

    [HttpGet()]
    public async Task<ActionResult<ChannelSettingsIdDto>> GetChannelSettings([FromQuery] int channelId, CancellationToken cancellationToken)
      => await Mediator.Send(new GetChannelSettingsQuery {
        ChannelSettingsId = channelId
      }, cancellationToken);

    [HttpGet("MyChannels/available")]
    public async Task<IEnumerable<SimpleChannelDTO>> GetMyAvailableChannels(CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetMyAvailableChannelsQuery {}, cancellationToken);
    }

    [HttpPut("UpdateChannelPause")]
    public async Task<ActionResult> UpdateChannelState([FromBody] UpdateChannelPauseCommand command, CancellationToken cancellationToken)
    {
      await Mediator.Send(command, cancellationToken);
      return NoContent();
    }

    [HttpPut("UpdateChannelSettings/{id}")]
    public async Task<ActionResult> UpdateChannelSettings([FromRoute] int id, UpdateChannelSettingsCommand command, CancellationToken cancellationToken)
    {
      command.Id = id;
      await Mediator.Send(command, cancellationToken);
      return NoContent();
    }

    [HttpGet("{id}/rounds")]
    public async Task<IEnumerable<RoundSnipDto>> GetRounds([FromRoute] int id, CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetChannelRoundsQuery {
        ChannelId = id
      }, cancellationToken);
    }

    [HttpGet("{id}/rounds/range/{startDate}/{endDate}")]
    public async Task<IEnumerable<RoundSnipDto>> GetRoundsInRange([FromRoute] int id, [FromRoute] DateTimeOffset startDate, [FromRoute] DateTimeOffset endDate, CancellationToken cancellationToken)
    {
      var command = new GetChannelRoundsInRangeQuery() {
        ChannelId = id,
        StartDate = startDate,
        EndDate = endDate
      };
      return await Mediator.Send(command, cancellationToken);
    }

    [HttpGet("{id}/rounds/active")]
    public async Task<ActiveRoundDto> GetActiveRound([FromRoute] int id, CancellationToken cancellationToken)
    {
      return await Mediator.Send(new GetCurrentRoundQuery {
        ChannelId = id
      }, cancellationToken);
    }


  }
}
