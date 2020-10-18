using Godot;
using System;
using System.Collections.Generic;

using Object = Godot.Object;

namespace GodotRx.Internal
{
  internal sealed class InstanceTracker : Object
  {
    public static readonly string OnFreedMethod = nameof(OnTrackerFreed);

    private static readonly Dictionary<ulong, InstanceTracker> store = new Dictionary<ulong, InstanceTracker>();

    public event Action? Freed;

    private readonly int _id;

    private InstanceTracker() {}

    private InstanceTracker(Object target)
    {
      _id = Singleton.RegisterInstanceTracker(this, target);
    }

    public static InstanceTracker Of(Object target)
    {
      var instId = target.GetInstanceId();

      if (!store.TryGetValue(instId, out var tracker))
      {
        tracker = new InstanceTracker(target);
        store[instId] = tracker;
        tracker.Freed += () => store.Remove(instId);
      }

      return tracker;
    }

    public void OnTrackerFreed(int id)
    {
      if (_id == id)
      {
        Freed?.Invoke();
        this.DeferredFree();
      }
    }

    protected override void Dispose(bool disposing)
    {
      // GD.Print("InstanceTracker disposed");
      base.Dispose(disposing);
    }
  }
}