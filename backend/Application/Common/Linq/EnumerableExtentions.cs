using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Linq
{
  public static class EnumerableExtensions
  {
    public static bool None<T>(this IEnumerable<T> source,
      Func<T, bool> predicate)
    {
      return !source.Any(predicate);
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
      foreach (var item in items)
      {
        action(item);
      }
    }
  }
}
