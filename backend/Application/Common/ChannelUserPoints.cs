using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;


namespace Application.Common
{
  public class ChannelUserPoints
  {

    private readonly IApplicationDbContext context;

    public ChannelUserPoints(IApplicationDbContext context)
    {
      this.context = context;
    }

    public void Enqueue(string slackUserId, string slackChannelId)
    {
      BackgroundJob.Enqueue(() => GiveMemberAPoint(slackUserId, slackChannelId));
    }

    public void Enqueue(string slackUserId, int channelSettingsId)
    {
      BackgroundJob.Enqueue(() => GiveMemberAPoint(slackUserId, channelSettingsId));
    }

    public async Task GiveMemberAPoint(string slackUserId, string slackChannelId)
    {
      var member = await context.ChannelMembers
        .Include(x => x.ChannelSettings)
        .Where(x => x.SlackUserId == slackUserId && x.ChannelSettings.SlackChannelId == slackChannelId)
        .FirstOrDefaultAsync();

      if (member != null)
      {
        member.Points++;
        await context.SaveChangesAsync(CancellationToken.None);
      }
    }

    public async Task GiveMemberAPoint(string slackUserId, int channelSettingsId)
    {
      var member = await context.ChannelMembers
        .Include(x => x.ChannelSettings)
        .Where(x => x.SlackUserId == slackUserId && x.ChannelSettingsId == channelSettingsId)
        .FirstOrDefaultAsync();

      if (member != null)
      {
        member.Points++;
        await context.SaveChangesAsync(CancellationToken.None);
      }
    }
  }
}
