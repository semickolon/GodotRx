using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GodotRx
{
  public static class ReactiveExtensions
  {
    public static void CompleteAndDispose<T>(this Subject<T> subject)
    {
      subject.OnCompleted();
      subject.Dispose();
    }

    public static IObservable<U> OfType<T, U>(this IObservable<T> observable)
      where T : class
      where U : T
    {
      return observable
        .Where(e => e is U)
        .Select(e => (U) e);
    }
  }
}