using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Prizes.Common;

namespace Application.Common.Interfaces.Hubs
{
  public interface IPrizeHubService
  {
    Task NewPrize(PrizeIdDTO newPrize);
    Task UpdatedPrize(PrizeIdDTO newPrize);
  }
}
