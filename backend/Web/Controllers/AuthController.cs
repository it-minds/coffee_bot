using Application.Common;
using Microsoft.AspNetCore.Mvc;
using SlackAuth.Query.GetAuthUrl;
using SlackAuth.Query.GetUserFromCallback;
using System.Threading.Tasks;
using User.Query.CheckCurrentUser;

namespace Web.Controllers
{
  public class AuthController : ApiControllerBase
  {
    [HttpPut]
    public async Task<ActionResult<AuthUser>> CheckAuth()
    {
      return await Mediator.Send(new CheckCurrentUserQuery());
    }

    [HttpGet("login")]
    public async Task<ActionResult<bool>> Login()
    {
      var url = await Mediator.Send(new GetAuthUrlQuery());

      return Redirect(url);
    }

    [HttpGet("login-callback")]
    public async Task<ActionResult<bool>> LoginCallback([FromQuery] string code, [FromQuery] string state)
    {
      var token = await Mediator.Send(new GetUserFromCallbackQuery {
        Code = code
      });

      return Redirect("http://localhost:3000/logincallback?token="+token); // TODO firgure out if we are going to use env variable
    }
  }
}
