using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
  public interface IApplicationDbContext
  {
    DbSet<ChannelSettings> ChannelSettings { get; set; }
    DbSet<ChannelMember> ChannelMembers { get; set; }
    DbSet<CoffeeRound> CoffeeRounds { get; set; }
    DbSet<CoffeeRoundGroup> CoffeeRoundGroups { get; set; }
    DbSet<CoffeeRoundGroupMember> CoffeeRoundGroupMembers { get; set; }
    DbSet<Prize> Prizes { get; set; }
    DbSet<ClaimedPrize> ClaimedPrizes { get; set; }
    DbSet<ChannelNotice> ChannelNotices { get; set; }
    DbSet<PredefinedGroup> PredefinedGroups { get; set; }
    DbSet<PredefinedGroupMember> PredefinedGroupMembers { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}
