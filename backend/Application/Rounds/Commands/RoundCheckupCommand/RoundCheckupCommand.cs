using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rounds.Commands.RoundFinisherCommand;
using Slack.Interfaces;

namespace Application.Rounds.Commands.RoundCheckupCommand
{
  public class RoundCheckupCommand : IRequest<int>
  {
    public class RoundCheckupCommandHandler : IRequestHandler<RoundCheckupCommand, int>
    {
      private readonly ISlackClient _slackClient;
      private readonly IApplicationDbContext _applicationDbContext;

      public RoundCheckupCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext)
      {
        _slackClient = slackClient;
        _applicationDbContext = applicationDbContext;
      }

      public async Task<int> Handle(RoundCheckupCommand request, CancellationToken cancellationToken)
      {
        var activeRounds = await _applicationDbContext.CoffeeRounds
          .Include(x => x.ChannelSettings)
          .Include(x => x.CoffeeRoundGroups)
          .Where(x => x.Active)
          .Where(x => x.EndDate <= DateTimeOffset.UtcNow)
          .ToListAsync(cancellationToken);
        
        var message = Slack.Messages.MidwayMessage.Generate(0);
        message.Channel = "C025V0ZTTJQ";

        //await _slackClient.SendMessageToChannel(cancellationToken, "C025V0ZTTJQ", message.c);
        await _slackClient.SendMessageToChannel(cancellationToken, message);
        Console.WriteLine(message);
        Console.WriteLine("FINISHGED");


        //foreach (var round in activeRounds)
        //{
        //  var roundDur = round.ChannelSettings.DurationInDays;
        //  var remainingDays = (round.EndDate - DateTimeOffset.UtcNow).Days;
        //  var msg = BuildChannelMessage(round);

        //  //await _slackClient.SendMessageToChannel(cancellationToken, "C025V0ZTTJQ", "WHERE COFFEE SELFIE??");

        //}

        return 1;
      }

      private string BuildChannelMessage(CoffeeRound round)
      {
        var sb = new StringBuilder();

        sb
          .AppendLine("Curtain call ladies and gentlefolk. <!channel>.")
          .Append("Your success has been measured and I give you a solid 10! (for effort)");


        sb.Append("Information regarding your next round TBA. Have a wonderful day :heart:");

        return sb.ToString();
      }
    }

  }
}
