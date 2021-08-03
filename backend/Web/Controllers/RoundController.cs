using System.Threading;
using System.Threading.Tasks;
using Application.Rounds.DTO;
using Application.Rounds.GetRound;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class RoundController : ApiControllerBase
  {
    [HttpGet()]
    public async Task<ActiveRoundDto> GetRound([FromQuery] GetRoundQuery request, CancellationToken cancellationToken) =>
       await Mediator.Send(request, cancellationToken);
  }
}
