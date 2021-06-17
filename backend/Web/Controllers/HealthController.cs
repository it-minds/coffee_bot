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
  }
}
