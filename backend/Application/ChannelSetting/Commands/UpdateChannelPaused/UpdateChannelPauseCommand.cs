using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.ChannelSetting.Commands.UpdateChannelPaused
{
  public class UpdateChannelPauseCommand : IRequest
  {
    public UpdateChannelPauseInput Input { get; set; }

    public class UpdateChannelPauseCommandHandler : IRequestHandler<UpdateChannelPauseCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentUserService _currentUserService;

      public UpdateChannelPauseCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
      {
        _context = context;
        _currentUserService = currentUserService;
      }

      public async Task<Unit> Handle(UpdateChannelPauseCommand request, CancellationToken cancellationToken)
      {
        var channelMember = await _context.ChannelMembers
          .Where(e => e.SlackUserId == request.Input.SlackUserId
                      && e.ChannelSettingsId == request.Input.ChannelId)
          .FirstOrDefaultAsync(cancellationToken);
        if (channelMember == null) throw new NotFoundException(nameof(ChannelMember), request.Input);

        channelMember.OnPause = request.Input.Paused;
        _context.ChannelMembers.Update(channelMember);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}
