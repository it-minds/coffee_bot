using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.ImagePost.Commands.ImagePostCommand;
using Microsoft.AspNetCore.Mvc;
using Slack.DTO;

namespace Web.Controllers
{
  public class EventController : ApiControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<string>> AllEventSubscriber([FromBody] EventInput body)
    {
      if (body.Type == "url_verification") {
        return body.Challenge;
      }

      switch (body.Event.Type) {
        case "file_shared": {

            Mediator.Enqueue(new ImagePostCommand {
              Event = body.Event
            });
            break;
          }

        case "file_created": {

          break;
        }
      }

      return "";
    }
  }
}
