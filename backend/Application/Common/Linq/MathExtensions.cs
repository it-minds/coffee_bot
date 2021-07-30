using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public delegate T MyFunc<T>(ref T arg1, in T arg2);

  public static class MathExtensions
  {
    public static U RunningSum<T, U>(this IEnumerable<T> source, Func<T, U> countFunction, Func<U,U,U> sumFunction, in U startingValue)
    {
      U runningSum = startingValue;
      source.ForEach(item => runningSum = sumFunction(runningSum, countFunction(item)));
      return runningSum;
    }
    public static U RunningSum<T, U>(this IEnumerable<T> source, Func<T, U> countFunction,  Func<U,U,U> sumFunction) =>
      source.RunningSum(countFunction, sumFunction, default(U));

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate) =>
      source.Percent(dividendPredicate, _ignore => true);

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate, Func<T, bool> divisorPredicate) =>
      source.Count(divisorPredicate) == 0 ? 0m : (100m * source.Count(dividendPredicate)) / source.Count(divisorPredicate);
  }
}
