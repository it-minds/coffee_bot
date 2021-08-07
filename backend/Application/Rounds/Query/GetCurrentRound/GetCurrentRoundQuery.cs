using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Rounds.DTO;
using Application.Rounds.GetRound;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rounds.GetCurrentRound
{
  [AuthorizeAttribute]
  public class GetCurrentRoundQuery : IRequest<ActiveRoundDto>
  {
    public int ChannelId { get; set; }

    public class GetCurrentRoundQueryHandler : IRequestHandler<GetCurrentRoundQuery, ActiveRoundDto>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly IMediator mediator;

      public GetCurrentRoundQueryHandler(IApplicationDbContext applicationDbContext, IMediator mediator)
      {
        this.applicationDbContext = applicationDbContext;
        this.mediator = mediator;
      }

      public async Task<ActiveRoundDto> Handle(GetCurrentRoundQuery request, CancellationToken cancellationToken)
      {
        var roundId = await applicationDbContext.CoffeeRounds
          .Where(x => x.Active && x.ChannelId == request.ChannelId)
          .Select(x => x.Id)
          .FirstOrDefaultAsync();

        if (roundId == default(int)) return null;

        return await mediator.Send(new GetRoundQuery { RoundId = roundId });

      }
    }
  }
}
