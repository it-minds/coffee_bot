using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Views;
using SlackNet.Interaction;

namespace Application.Interactivity.View.SelectMemeberParticipation
{
  public class SelectMemeberParticipationCommand : IRequest<int>
  {
    public ViewSubmission ViewSubmission { get; set; }

    public class SelectMemeberParticipationCommandHandler : CommandBase, IRequestHandler<SelectMemeberParticipationCommand, int>
    {
      public SelectMemeberParticipationCommandHandler(IApplicationDbContext dbContext) : base(dbContext) { }

      public async Task<int> Handle(SelectMemeberParticipationCommand request, CancellationToken cancellationToken)
      {
        var participatingMembers = request.ViewSubmission.View.State.GetSelectedParticipants();

        string SenderId = request.ViewSubmission.User.Id;

        var group = await dbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRound)
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRound.Active && x.CoffeeRoundGroupMembers.Any(x => x.SlackMemberId == SenderId))
          .FirstOrDefaultAsync();

        foreach (var member in group.CoffeeRoundGroupMembers)
        {
          if(participatingMembers.Contains(member.SlackMemberId))
            member.Participated = true;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return 1;
      }
    }
  }
}
