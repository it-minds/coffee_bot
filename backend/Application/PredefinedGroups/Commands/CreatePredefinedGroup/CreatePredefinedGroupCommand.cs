using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CreatePredefinedGroups.Commands.CreatePredefinedGroup
{

  public class CreatePredefinedGroupCommand : IRequest<int>
  {
    public int ChannelId { get; set; }
    public IEnumerable<int> MemberIds { get; set; }

    public class CreatePredefinedGroupCommandHandler : CommandBase, IRequestHandler<CreatePredefinedGroupCommand, int>
    {
      public CreatePredefinedGroupCommandHandler(IApplicationDbContext dbContext) : base(dbContext) { }

      public async Task<int> Handle(CreatePredefinedGroupCommand request, CancellationToken cancellationToken)
      {
        var channel = await dbContext.ChannelSettings.Where(x => x.Id == request.ChannelId).FirstOrDefaultAsync(cancellationToken);

        if (channel == null)
        {
          throw new ArgumentException("");
        }

        var allMembersExists = !(await dbContext.ChannelMembers.AnyAsync(x =>
          x.ChannelSettingsId == channel.Id &&
          !request.MemberIds.Any(y => y == x.Id)
        , cancellationToken));

        if (!allMembersExists)
        {
          throw new ArgumentException("");
        }

        var groupMemberExists = await dbContext.PredefinedGroupMembers.AnyAsync(x => request.MemberIds.Contains(x.Id), cancellationToken);

        if (groupMemberExists)
        {
          throw new ArgumentException("");
        }

        var group = new PredefinedGroup
        {
          ChannelSettingsId = channel.Id,
          PredefinedGroupMembers = request.MemberIds.Select(x => new PredefinedGroupMember {
            ChannelMemberId = x
          }).ToList()
        };

        dbContext.PredefinedGroups.Add(group);
        await dbContext.SaveChangesAsync(cancellationToken);

        return group.Id;
      }
    }
  }
}
