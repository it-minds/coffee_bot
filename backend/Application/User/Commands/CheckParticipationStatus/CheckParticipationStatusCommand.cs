using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Commands.CheckParticipationStatus
{
  public class CheckParticipationStatusCommand : IRequest<int>
  {
    public class CheckParticipationStatusCommandHandler : CommandBase, IRequestHandler<CheckParticipationStatusCommand, int>
    {
      public CheckParticipationStatusCommandHandler(IApplicationDbContext dbContext) : base(dbContext) {}

      public async Task<int> Handle(CheckParticipationStatusCommand request, CancellationToken cancellationToken)
      {
        var now = System.DateTimeOffset.UtcNow;
        var membersToReenable = await dbContext.ChannelMembers
          .Where(x => x.OnPause && x.ReturnFromPauseDate != null && x.ReturnFromPauseDate < now)
          .ToListAsync();

        foreach (var member in membersToReenable)
        {
          member.OnPause = false;
          member.ReturnFromPauseDate = null;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return 0;
      }
    }
  }
}
