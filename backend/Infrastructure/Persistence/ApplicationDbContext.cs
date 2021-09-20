using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
  public class ApplicationDbContext : DbContext, IApplicationDbContext
  {
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeOffsetService _dateTimeOffsetService;

    public ApplicationDbContext(
        DbContextOptions options,
        ICurrentUserService currentUserService,
        IDateTimeOffsetService dateTimeOffset) : base(options)
    {
      _currentUserService = currentUserService;
      _dateTimeOffsetService = dateTimeOffset;
    }

    public DbSet<ChannelSettings> ChannelSettings { get; set; }
    public DbSet<ChannelMember> ChannelMembers { get; set; }
    public DbSet<CoffeeRound> CoffeeRounds { get; set; }
    public DbSet<CoffeeRoundGroup> CoffeeRoundGroups { get; set; }
    public DbSet<CoffeeRoundGroupMember> CoffeeRoundGroupMembers { get; set; }
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<ClaimedPrize> ClaimedPrizes { get; set; }
    public DbSet<ChannelNotice> ChannelNotices { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      // foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
      // {
      //   switch (entry.State)
      //   {
      //     case EntityState.Added:
      //       entry.Entity.CreatedBy = _currentUserService.UserId;
      //       entry.Entity.Created = _dateTimeOffsetService.Now;
      //       break;
      //     case EntityState.Modified:
      //       entry.Entity.LastModifiedBy = _currentUserService.UserId;
      //       entry.Entity.LastModified = _dateTimeOffsetService.Now;
      //       break;
      //   }
      // }

      return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(builder);
    }
  }
}
