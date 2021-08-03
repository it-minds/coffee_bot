using System.Collections.Generic;
using Application.Common;
using Xunit;

namespace Application.UnitTests.Common
{
  public class WordStringsTests
  {
    [Fact]
    public void GiveMemberAPointShouldBeUnique()
    {
      var wordStrings = new WordStrings();

      var results = new Dictionary<string, int>();

      for (int i = 0; i < wordStrings.MaxCount; i++)
      {
        var result = wordStrings.GetPredeterminedStringFromInt(i);
        if (results.ContainsKey(result))
        {
          var oldI = results.GetValueOrDefault(result);
          Assert.True(false, $"Result contained {result} at {i} old at {oldI}");
          return;
        }
        results.Add(result, i);
      }
      Assert.True(true);
    }
  }
}
