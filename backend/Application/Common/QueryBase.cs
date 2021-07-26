using Application.Common.Interfaces;
using AutoMapper;

namespace Application.Common
{
  public abstract class QueryBase: CommandBase
  {
    protected readonly IMapper mapper;

    public QueryBase(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
    {
      this.mapper = mapper;
    }
  }
}
