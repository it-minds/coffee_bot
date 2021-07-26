using Application.Common.Interfaces;

namespace Application.Common
{
  public abstract class CommandBase
  {
    protected readonly IApplicationDbContext dbContext;

    public CommandBase(IApplicationDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
  }
}
