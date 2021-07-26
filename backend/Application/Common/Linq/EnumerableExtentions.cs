using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public static class EnumerableExtensions
  {
    public static bool None<TSource>(this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return !source.Any(predicate);
    }
  }
}
