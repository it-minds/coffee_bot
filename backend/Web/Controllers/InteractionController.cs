using System.Threading.Tasks;
using Application.Common.Hangfire.MediatR;
using Application.Interactivity.Block.GenericBlockResponse;
using Application.Interactivity.View.GenericViewResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlackNet;
using SlackNet.Interaction;

namespace Web.Controllers
{
  public class InteractionController : ApiControllerBase
  {
    private readonly SlackJsonSettings jsonSettings;
    public InteractionController() {
      this.jsonSettings = new SlackServiceBuilder().GetJsonSettings();
    }

    [HttpPost("")]
    public async Task InteractionResponse([FromForm] PayloadWrapper body)
    {
      var request = JsonConvert.DeserializeObject<InteractionRequest>(body.Payload, jsonSettings.SerializerSettings);

      switch (request) {
        case BlockActionRequest blockActionRequest: {
          Mediator.Enqueue(new GenericBlockResponseCommand {
            BlockActionRequest = blockActionRequest
          });
          break;
        }

        case ViewSubmission viewSubmission: {
          Mediator.Enqueue(new GenericViewResponseCommand {
            ViewSubmission = viewSubmission
          });
          break;
        }
      }
    }
  }

  public class PayloadWrapper {
    public string Payload { get; set; }
  }
}
