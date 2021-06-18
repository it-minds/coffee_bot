using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ChannelSetting;
using Application.ChannelSetting.Commands.UpdateChannelPaused;
using Application.ChannelSetting.Queries.GetMyChannelsSettings;
using Application.ChannelSync.Commands;
using Application.Common.Hangfire.MediatR;
using Microsoft.AspNetCore.Mvc;
using Rounds.Commands.RoundInitiatorCommand;

namespace Web.Controllers
{
  public class ChannelController : ApiControllerBase
  {
    [HttpGet("MyChannels")]
    public async Task<ActionResult<List<ChannelSettingsIdDto>>> GetMyChannels()
    {
      return await Mediator.Send(new GetMyChannelSettingsQuery());
    }

    [HttpPut("UpdateChannelState")]
    public async Task<ActionResult> UpdateChannelState(UpdateChannelPauseCommand command)
    {
      await Mediator.Send(command);
      return NoContent();
    }

    //[HttpPost("round-init")]
    //public async Task<ActionResult<bool>> RoundInitiator(RoundInitiatorCommand command)
    //{
    //  await Mediator.Send(command);

    //  return true;
    //}
  }
}
