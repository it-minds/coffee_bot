using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class SlashController : ApiControllerBase
  {
    [HttpPost("coffee-group-done")]
    public async Task<ActionResult<bool>> TEST([FromBody] dynamic body)
    {

      System.Console.WriteLine("whatamI?" + body);

      return true;
    }
  }
}
