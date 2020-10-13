using Godot;
using GodotRx.Internal;
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

    public static IObservable<float> OnProcess(this Node node)
      => node.GetNodeTracker().OnProcess;

    public static IObservable<float> OnPhysicsProcess(this Node node)
      => node.GetNodeTracker().OnPhysicsProcess;

    public static IObservable<InputEvent> OnInput(this Node node)
      => node.GetNodeTracker().OnInput;
    
    public static IObservable<InputEvent> OnUnhandledInput(this Node node)
      => node.GetNodeTracker().OnUnhandledInput;

    public static IObservable<InputEventKey> OnUnhandledKeyInput(this Node node)
      => node.GetNodeTracker().OnUnhandledKeyInput;

    private static NodeTracker GetNodeTracker(this Node node)
    {
      var tracker = node.GetNodeOrNull<NodeTracker>(NodeTracker.DefaultName);

      if (tracker == null)
      {
        tracker = new NodeTracker();
        tracker.Name = NodeTracker.DefaultName;
        node.AddChild(tracker);
      }

      return tracker;
    }

    public static IObservable<InputEventMouseButton> OnMouseDown(this Node node, ButtonList button = Godot.ButtonList.Left)
      => node.OnMouseButtonEvent(false, button, true);

    public static IObservable<InputEventMouseButton> OnMouseUp(this Node node, ButtonList button = Godot.ButtonList.Left)
      => node.OnMouseButtonEvent(false, button, false);

    public static IObservable<InputEventMouseButton> OnUnhandledMouseDown(this Node node, ButtonList button = Godot.ButtonList.Left)
      => node.OnMouseButtonEvent(true, button, true);

    public static IObservable<InputEventMouseButton> OnUnhandledMouseUp(this Node node, ButtonList button = Godot.ButtonList.Left)
      => node.OnMouseButtonEvent(true, button, false);

    private static IObservable<InputEventMouseButton> OnMouseButtonEvent(this Node node, bool unhandled, ButtonList button, bool pressed)
    {
      return (unhandled ? node.OnUnhandledInput() : node.OnInput())
        .OfType<InputEvent, InputEventMouseButton>()
        .Where(ev => ev.ButtonIndex == (int) button && ev.Pressed == pressed);
    }

    public static IObservable<InputEventKey> OnKeyPressed(this Node node, KeyList key)
      => node.OnKeyEvent(false, key, true, null);

    public static IObservable<InputEventKey> OnKeyReleased(this Node node, KeyList key)
      => node.OnKeyEvent(false, key, false, null);

    public static IObservable<InputEventKey> OnKeyJustPressed(this Node node, KeyList key)
      => node.OnKeyEvent(false, key, true, false);

    public static IObservable<InputEventKey> OnUnhandledKeyPressed(this Node node, KeyList key)
      => node.OnKeyEvent(true, key, true, null);

    public static IObservable<InputEventKey> OnUnhandledKeyReleased(this Node node, KeyList key)
      => node.OnKeyEvent(true, key, false, null);

    public static IObservable<InputEventKey> OnUnhandledKeyJustPressed(this Node node, KeyList key)
      => node.OnKeyEvent(true, key, true, false);

    private static IObservable<InputEventKey> OnKeyEvent(this Node node, bool unhandled, KeyList key, bool pressed, bool? echo)
    {
      return (unhandled ? node.OnUnhandledInput() : node.OnInput())
        .OfType<InputEvent, InputEventKey>()
        .Where(ev => ev.Scancode == (uint) key && ev.Pressed == pressed && (echo == null || ev.Echo == echo));
    }

    public static IObservable<Unit> OnActionPressed(this Node node, string action)
    {
      return node.OnProcess()
        .Where(_ => Input.IsActionPressed(action))
        .Select(_ => new Unit());
    }

    public static IObservable<Unit> OnActionJustPressed(this Node node, string action)
    {
      return node.OnProcess()
        .Where(_ => Input.IsActionJustPressed(action))
        .Select(_ => new Unit());
    }

    public static IObservable<Unit> OnActionJustReleased(this Node node, string action)
    {
      return node.OnProcess()
        .Where(_ => Input.IsActionJustReleased(action))
        .Select(_ => new Unit());
    }
  }
}