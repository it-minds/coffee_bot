using Domain.Entities;

namespace Application.Rounds.Extensions
{
  public static class CoffeeRoundGroupExtensions
  {
    public static int GroupScore(this CoffeeRoundGroup group)
    {
      var score = 0;

      if (group.HasMet) score += 1;
      if (group.HasPhoto) score += 2;
      if (group.FinishedAt < group.CoffeeRound?.EndDate) score += 1;

      return score;
    }
  }
}
