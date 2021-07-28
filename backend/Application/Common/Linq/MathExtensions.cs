using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public static class MathExtensions
  {
    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate) =>
      source.Percent(dividendPredicate, _ignore => true);

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividendPredicate, Func<T, bool> divisorPredicate) =>
      source.Count(divisorPredicate) == 0 ? 0m : (100m * source.Count(dividendPredicate)) / source.Count(divisorPredicate);
  }
}
