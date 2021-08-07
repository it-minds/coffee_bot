using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.ChannelSync.Commands.ChannelMemberPointsSync.Extensions;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ChannelSync.Commands.ChannelMemberPointsSync
{
  public class ChannelMemberPointsSyncCommand : IRequest<string>
  {
    public class ChannelMemberPointsSyncCommandHandler : CommandBase, IRequestHandler<ChannelMemberPointsSyncCommand, string>
    {
      public ChannelMemberPointsSyncCommandHandler(IApplicationDbContext dbContext) : base(dbContext) {}

      public async Task<string> Handle(ChannelMemberPointsSyncCommand request, CancellationToken cancellationToken)
      {
        var channelMembers = await dbContext.ChannelMembers
          .Where(x => !x.IsRemoved)
          .ToListAsync(cancellationToken);

        var coffeeGroupMembers = await dbContext.CoffeeRoundGroupMembers
          .Include(x => x.CoffeeRoundGroup)
            .ThenInclude(x => x.CoffeeRound)
          .ToListAsync(cancellationToken);

        var result = new StringBuilder("");

        foreach (var channelMember in channelMembers)
        {
          var shouldHavePoints = channelMember.RoundScore(coffeeGroupMembers);
          if (shouldHavePoints != channelMember.Points)
          {
            result.Append(channelMember.SlackName).Append(" should have had p").Append(shouldHavePoints)
              .Append( "but has p").Append(channelMember.Points).AppendLine("!");
            channelMember.Points = shouldHavePoints;
          }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return result.ToString();
      }
    }
  }
}
