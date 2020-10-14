using Godot;
using GodotRx.Internal;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace GodotRx
{
  public static class NodeExtensions
  {
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