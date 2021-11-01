using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
      RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
      int n = list.Count;
      while (n > 1)
      {
        byte[] box = new byte[1];
        do provider.GetBytes(box);
        while (!(box[0] < n * (Byte.MaxValue / n)));
        int k = (box[0] % n);
        n--;
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
      return list;
    }
  }
}
