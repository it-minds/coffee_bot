using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.ImagePost.Commands.ImagePostCommand;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Slack.DTO;
using SlackNet;
using SlackNet.Events;
using Web.Binders;

namespace Web.Controllers
{
  public class EventController : ApiControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<string>> AllEventSubscriber([FromBody] JObject body)
    {
      var request = body.ToObject<EventRequest>( );

      if (request.Type == "url_verification") {
        var urlVerification = body.ToObject<UrlVerification>( );
        return urlVerification.Challenge;
      }

      var callback = body.ToObject<EventCallback>();

      switch (callback.Event.Type) {
        case "file_shared" : {
          var test = body.ToObject<EventCallback<MyFileShared>>();

          Mediator.Enqueue(new ImagePostCommand {
            Event = test.Event
          });
          break;
        }
      }

      return "";
    }


  }
}
