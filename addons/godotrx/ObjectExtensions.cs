using Godot;
using GodotRx.Internal;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Object = Godot.Object;

namespace GodotRx
{
  public static class ObjectExtensions
  {
    public static IObservable<Unit> ObserveSignal(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker());

    public static IObservable<T> ObserveSignal<T>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T>());

    public static IObservable<(T1, T2)> ObserveSignal<T1, T2>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T1, T2>());

    public static IObservable<(T1, T2, T3)> ObserveSignal<T1, T2, T3>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3>());

    public static IObservable<(T1, T2, T3, T4)> ObserveSignal<T1, T2, T3, T4>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4>());

    public static IObservable<(T1, T2, T3, T4, T5)> ObserveSignal<T1, T2, T3, T4, T5>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4, T5>());

    public static IObservable<(T1, T2, T3, T4, T5, T6)> ObserveSignal<T1, T2, T3, T4, T5, T6>(this Object obj, string signalName)
      => ObserveSignal(obj, signalName, new EventTracker<T1, T2, T3, T4, T5, T6>());

    private static IObservable<T> ObserveSignal<T>(Object obj, string signalName, BaseEventTracker<T> tracker)
    {
      obj.Connect(signalName, tracker, tracker.TargetMethod);

      var subscriptionList = new List<IDisposable>();
      var onSignal = tracker.OnSignal;
      var instId = obj.GetInstanceId();

      InstanceTracker.Of(obj).Freed += () =>
      {
        // GD.Print($"freed {instId}:{signalName}");
        subscriptionList.ForEach(s => s.Dispose());
        tracker.DeferredFree();
      };

      return Observable.Create<T>(observer =>
      {
        var subscription = onSignal.Subscribe(observer.OnNext);
        subscriptionList.Add(subscription);

        return Disposable.Create(() =>
        {
          subscription.Dispose();
          subscriptionList.Remove(subscription);
        });
      });
    }

    public static void DeferredFree(this Object obj)
    {
      obj.CallDeferred("free");
    }

    public static IObservable<Unit> OnFramePreDraw(this Object obj)
      => VisualServerSignals.OnFramePreDraw();

    public static IObservable<Unit> OnNextFramePreDraw(this Object obj)
      => obj.OnFramePreDraw().Take(1);

    public static Task WaitNextFramePreDraw(this Object obj)
      => obj.OnNextFramePreDraw().ToTask();

    public static IObservable<Unit> OnFramePostDraw(this Object obj)
      => VisualServerSignals.OnFramePostDraw();

    public static IObservable<Unit> OnNextFramePostDraw(this Object obj)
      => obj.OnFramePostDraw().Take(1);

    public static Task WaitNextFramePostDraw(this Object obj)
      => obj.OnNextFramePostDraw().ToTask();
  }
}