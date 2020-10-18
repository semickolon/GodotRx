using System;
using System.Collections.Generic;

namespace GodotRx.Internal
{
  internal static class ListExtensions
  {
    internal static void SafeForEach<T>(this List<T> list, Action<T> action)
    {
      foreach (var e in list.ToArray())
        action(e);
    }
  }
}
