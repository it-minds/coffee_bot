using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public static class MathExtensions
  {
    public static int RunningSum<T>(this IEnumerable<T> source, Func<T, int> countFunction, Func<int, int, int> sumFunction)
    {
      var runningSum = 0;
      foreach (var item in source)
      {
        var count = countFunction(item);
        runningSum = sumFunction(count, runningSum);
      }
      return runningSum;
    }

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate) =>
      source.Percent(dividendPredicate, _ignore => true);

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate, Func<T, bool> divisorPredicate) =>
      source.Count(divisorPredicate) == 0 ? 0m : (100m * source.Count(dividendPredicate)) / source.Count(divisorPredicate);
  }
}
