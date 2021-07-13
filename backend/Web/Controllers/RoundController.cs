using System.Threading.Tasks;
using Application.Rounds.DTO;
using Application.Rounds.GetRound;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class RoundController : ApiControllerBase
  {
    [HttpGet("{id}")]
    public async Task<ActiveRoundDto> GetRound([FromRoute] int id)
    {
      return await Mediator.Send(new GetRoundQuery {
        RoundId = id
      });
    }
  }
}
