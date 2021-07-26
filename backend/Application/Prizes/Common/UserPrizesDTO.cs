using System.Collections.Generic;
namespace Application.Prizes.Common
{
  public class UserPrizesDTO
  {
    public string SlackUserId { get; set; }
    public int Points { get; set; }
    public int PointsRemaining { get; set; }

    public IEnumerable<ClaimedPrizeDTO> PrizesClaimed {get; set;}
    public IEnumerable<PrizeIdDTO> PrizesAvailable {get; set;}

  }
}
