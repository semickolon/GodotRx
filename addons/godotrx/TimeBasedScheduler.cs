using Godot;
using GodotRx.Internal;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace GodotRx
{
  public class TimeBasedScheduler : IScheduler
  {
    public static readonly TimeBasedScheduler Instance = new TimeBasedScheduler();

    public DateTimeOffset Now => DateTimeOffset.FromUnixTimeMilliseconds((long) OS.GetSystemTimeMsecs());

    private TimeBasedScheduler() {}

    private void DelayAction<TState>(TimeSpan dueTime, TState state, BooleanDisposable disposable, Func<IScheduler, TState, IDisposable> action)
    {
      Singleton.SceneTree
        .CreateTimer((float) dueTime.TotalSeconds, false)
        .OnTimeout()
        .Take(1)
        .Subscribe(_ =>
        {
          if (!disposable.IsDisposed)
          {
            action(this, state);
          }
        });
    }

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
      return DefaultScheduler.Instance.Schedule(state, action);
    }

    public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
      if (dueTime.TotalSeconds <= 0)
      {
        return Schedule(state, action);
      }

      var disposable = new BooleanDisposable();
      DelayAction(dueTime, state, disposable, action);
      return disposable;
    }

    public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
    {
      return Schedule(state, dueTime - Now, action);
    }
  }
}