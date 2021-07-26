using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  public class HealthController : ApiControllerBase
  {
    [HttpGet]
    public ActionResult<bool> GetBackendHealth()
    {
      // TODO make integration health checks

      return true;
    }

    [HttpGet("cancel")]
    public ActionResult<bool> GetCancelTest(CancellationToken token)
    {
      Thread.Sleep(2000);

      // token.ThrowIfCancellationRequested();

      return !token.IsCancellationRequested;
    }
  }
}
