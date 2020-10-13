using Godot;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

using Object = Godot.Object;

namespace GodotRx
{
  public static class NodeExtensions
  { 
    public static IObservable<Unit> ObserveSignal(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker());
    
    public static IObservable<T> ObserveSignal<T>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T>());

    public static IObservable<(T1, T2)> ObserveSignal<T1, T2>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T1, T2>());

    public static IObservable<(T1, T2, T3)> ObserveSignal<T1, T2, T3>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3>());

    public static IObservable<(T1, T2, T3, T4)> ObserveSignal<T1, T2, T3, T4>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4>());

    public static IObservable<(T1, T2, T3, T4, T5)> ObserveSignal<T1, T2, T3, T4, T5>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4, T5>());

    public static IObservable<(T1, T2, T3, T4, T5, T6)> ObserveSignal<T1, T2, T3, T4, T5, T6>(this Object obj, string signalName)
      => _ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4, T5, T6>());

    private static IObservable<T> _ObserveSignal<T>(Object obj, string signalName, BaseEventTracker<T> tracker)
    {
      obj.Connect(signalName, tracker, tracker.TargetMethod);

      var subscriptionList = new List<IDisposable>();
      var onSignal = tracker.OnSignal;
      var instId = obj.GetInstanceId();
      
      obj.OnFreed().Subscribe(_ =>
      {
        // GD.Print($"freed {instId}:{signalName}");
        subscriptionList.ForEach(s => s.Dispose());
        tracker.DeferredFree();
      });

      return Observable.Create<T>(observer => 
      {
        var subscription = onSignal.Subscribe(observer.OnNext);
        subscriptionList.Add(subscription);

        return () =>
        {
          subscription.Dispose();
          subscriptionList.Remove(subscription);
        };
      });
    }

    public static void DeferredFree(this Object obj)
    {
      obj.CallDeferred("free");
    }

    private static IObservable<Unit> OnFreed(this Object obj)
    {
      var tracker = new InstanceTracker(obj);
      return Observable.FromEvent(
        d => tracker.Freed += d,
        d => tracker.Freed -= d
      );
    }
  }
}