using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Gallery.Queries.GetAllPhotos;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

  public class GalleryController : ApiControllerBase
  {
    [HttpGet("all")]
    public async Task<ActionResult<List<StandardGroupDto>>> GetAll(GetAllPhotosQuery request, CancellationToken cancellationToken) =>
      await Mediator.Send(request, cancellationToken);
  }
}
