using System.Threading.Tasks;
using Application.ChannelSync.Commands;
using Application.Common.Hangfire.MediatR;
using Application.Rounds.Commands.RoundCheckupCommand;
using Microsoft.AspNetCore.Mvc;
using Rounds.Commands.RoundInitiatorCommand;

namespace Web.Controllers
{
  public class TestController : ApiControllerBase
  {
    [HttpPost("channel-sync")]
    public async Task<ActionResult<bool>> ChannelSync(SyncronizeChannelsCommand command)
    {
      await Mediator.Send(command);

      return true;
    }

    [HttpPost("new-channel-msg")]
    public ActionResult<bool> NewChannelMessager(NewChannelMessagerCommand command)
    {
      Mediator.Enqueue(command);

      return true;
    }

    [HttpPost("round-init")]
    public async Task<ActionResult<bool>> RoundInitiator(RoundInitiatorCommand command)
    {
      await Mediator.Send(command);

      return true;
    }
    [HttpPost("round-checkup")]
    public async Task<ActionResult<bool>> RoundCheckup(RoundCheckupCommand command)
    {
      await Mediator.Send(command);

      return true;
    }
  }
}
