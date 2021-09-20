using System.Threading;
using System.Threading.Tasks;
using Application.ChannelNotices.Commands.CreateChannelNotice;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class ChannelNoticesController : ApiControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<int>> Test([FromBody] CreateChannelNoticeCommand body, CancellationToken cancellationToken)
     => await Mediator.Send(body, cancellationToken);
  }
}
