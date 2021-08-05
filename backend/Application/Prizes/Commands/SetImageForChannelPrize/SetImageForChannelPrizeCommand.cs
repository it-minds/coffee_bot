using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Hubs;
using Application.Common.Security;
using Application.Common.SignalR.Hub;
using Application.Prizes.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Prizes.Commands.SetImageForChannelPrize
{

  [Authorize]
  public class SetImageForChannelPrizeCommand : IRequest<string>
  {
    public IFormFile file { get; set; }
    public int PrizeId { get; set; }

    public class SetImageForChannelPrizeCommandHandler : CommandBase, IRequestHandler<SetImageForChannelPrizeCommand, string>
    {
      private readonly WordStrings wordStrings;
      private readonly ICurrentUserService currentUserService;
      private readonly IHubContext<PrizeHub, IPrizeHubService> hubContext;
      private readonly IMapper mapper;


      public SetImageForChannelPrizeCommandHandler(IApplicationDbContext dbContext, WordStrings wordStrings, ICurrentUserService currentUserService, IHubContext<PrizeHub, IPrizeHubService> hubContext, IMapper mapper) : base(dbContext)
      {
        this.wordStrings = wordStrings;
        this.currentUserService = currentUserService;
        this.hubContext = hubContext;
        this.mapper = mapper;
      }

      public async Task<string> Handle(SetImageForChannelPrizeCommand request, CancellationToken cancellationToken)
      {
        var prize = await dbContext.Prizes
          .Where(x => x.Id == request.PrizeId)
          .FirstOrDefaultAsync(cancellationToken);

        var channelMember = await dbContext.ChannelMembers
          .Where(x => x.IsAdmin &&
            x.ChannelSettingsId == prize.ChannelSettingsId &&
            x.SlackUserId == currentUserService.UserSlackId)
          .FirstOrDefaultAsync(cancellationToken);

        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), currentUserService.UserSlackId );

        var newName = wordStrings.GetPredeterminedStringFromInt(
          prefix: "Prize",
          x: prize.Id,
          suffix: Path.GetExtension(request.file.FileName)
        ).ToLower();

        using (var fs = File.Create("wwwroot/images/prizes/" + newName))
        {
          await request.file.CopyToAsync(fs, cancellationToken);
        }

        prize.ImageName = newName;
        await dbContext.SaveChangesAsync(cancellationToken);

        await hubContext.Clients.All.UpdatedPrize(mapper.Map<PrizeIdDTO>(prize));


        return newName;
      }
    }
  }
}
