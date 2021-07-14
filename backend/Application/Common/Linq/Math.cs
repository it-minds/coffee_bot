using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public static class Math
  {
    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividenPredicate, Func<T, bool> divisorPredicate) =>
      source.Count(x => divisorPredicate(x)) == 0 ? 0m : (100m * source.Count(x => dividenPredicate(x) )) / source.Count(x => divisorPredicate(x));

    public static decimal Percent<T>(this IEnumerable<T> source, Func<T, bool> dividenPredicate ) =>
      source.Count() == 0 ? 0m : (100m * source.Count(x => dividenPredicate(x))) / source.Count();

  }
}
