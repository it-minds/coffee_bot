using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Hubs;
using Application.Common.SignalR.Hub;
using Application.Prizes.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Prizes.Commands.CreateChannelPrize
{
  public class CreateChannelPrizeCommand : IRequest<int>
  {
    public PrizeDTO Input {get; set; }

    public class CreateChannelPrizeCommandHandler : IRequestHandler<CreateChannelPrizeCommand, int>
    {
      private readonly IApplicationDbContext dbContext;
      private readonly IMapper mapper;
      private readonly IHubContext<PrizeHub, IPrizeHubService> hubContext;
      public CreateChannelPrizeCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IHubContext<PrizeHub, IPrizeHubService> hubContext, ICurrentUserService currentUserService)
      {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.hubContext = hubContext;
      }

      public async Task<int> Handle(CreateChannelPrizeCommand request, CancellationToken cancellationToken)
      {

        var prize = new Prize
        {
          ChannelSettingsId = request.Input.ChannelSettingsId,
          PointCost = request.Input.PointCost,
          IsMilestone = request.Input.IsMilestone,
          IsRepeatable = request.Input.IsRepeatable,
          Title = request.Input.Title,
          Description = request.Input.Description,
        };

        if (!prize.IsValid) throw new ArgumentException("Invalid Prize");

        dbContext.Prizes.Add(prize);

        await dbContext.SaveChangesAsync(cancellationToken);

        await hubContext.Clients.All.NewPrize(mapper.Map<PrizeIdDTO>(prize));

        return 1;
      }
    }
  }
}
