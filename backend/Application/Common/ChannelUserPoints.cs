using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
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

    public void Enqueue(string slackUserId, string slackChannelId, int points = 1)
    {
      BackgroundJob.Enqueue(() => GiveMemberAPoint(slackUserId, slackChannelId, points));
    }

    public void Enqueue(string slackUserId, int channelSettingsId, int points = 1)
    {
      BackgroundJob.Enqueue(() => GiveMemberAPoint(slackUserId, channelSettingsId, points));
    }

    public void Enqueue(IEnumerable<string> members, int channelSettingsId, int points = 1)
    {
      BackgroundJob.Enqueue(() => GiveMemberAPoint(members.ToArray(), channelSettingsId, points));
    }

    public async Task GiveMemberAPoint(string slackUserId, string slackChannelId, int points = 1)
    {
      var member = await context.ChannelMembers
        .Include(x => x.ChannelSettings)
        .Where(x => x.SlackUserId == slackUserId && x.ChannelSettings.SlackChannelId == slackChannelId)
        .ToListAsync();

      await AddPoints(member, points);
    }

    public async Task GiveMemberAPoint(string slackUserId, int channelSettingsId , int points = 1)
    {
      var member = await context.ChannelMembers
        .Include(x => x.ChannelSettings)
        .Where(x => x.SlackUserId == slackUserId && x.ChannelSettingsId == channelSettingsId)
        .ToListAsync();

      await AddPoints(member, points);
    }

    public async Task GiveMemberAPoint(IEnumerable<string> members, int channelSettingsId , int points = 1)
    {
      var member = await context.ChannelMembers
        .Include(x => x.ChannelSettings)
        .Where(x => members.Contains(x.SlackUserId) && x.ChannelSettingsId == channelSettingsId)
        .ToListAsync();

      await AddPoints(member, points);
    }

    private async Task AddPoints(IEnumerable<ChannelMember> members, int pointsToGive)
    {
      foreach (var member in members)
      {
        member.Points++;
      }
      await context.SaveChangesAsync(CancellationToken.None);
    }
  }
}
