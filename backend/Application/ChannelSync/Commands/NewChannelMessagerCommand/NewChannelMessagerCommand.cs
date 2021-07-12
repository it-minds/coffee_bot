using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slack.Interfaces;

namespace Application.ChannelSync.Commands
{
  public class NewChannelMessagerCommand : IRequest<int>
  {

    public string SlackChannelId { get; set; }

    public class NewChannelMessagerCommandHandler : IRequestHandler<NewChannelMessagerCommand, int>
    {
      private readonly ISlackClient slackClient;
      private readonly IApplicationDbContext applicationDbContext;

      public NewChannelMessagerCommandHandler(ISlackClient slackClient, IApplicationDbContext applicationDbContext) {
        this.slackClient = slackClient;
        this.applicationDbContext = applicationDbContext;
      }


      public async Task<int> Handle(NewChannelMessagerCommand request, CancellationToken cancellationToken)
      {
        var channelSettings = await applicationDbContext.ChannelSettings.FirstOrDefaultAsync(x => x.SlackChannelId == request.SlackChannelId);

        if (channelSettings == null) return 0;

        await slackClient.SendMessageToChannel(cancellationToken, channelSettings.SlackChannelId, "This <!channel> is now part of the coffee rounds! Changes to the channel settings is currenly not supported. Please `/leave` this channel if you nolonger wish to partake! First round should start shortly.");

        return 1;
      }
    }
  }
}
