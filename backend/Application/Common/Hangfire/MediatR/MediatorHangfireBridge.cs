using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;

namespace Application.Common.Hangfire.MediatR
{
    public class MediatorHangfireBridge
    {
      private readonly IMediator _mediator;

      public MediatorHangfireBridge(IMediator mediator)
      {
        _mediator = mediator;
      }

      public async Task<object?> Send(IBaseRequest command)
      {
        return await _mediator.Send(command);
      }

      [DisplayName("{0}")]
      public async Task<object?> Send(string jobName, IBaseRequest command)
      {
        return await _mediator.Send(command);
      }
    }
}
