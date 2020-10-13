using Godot;
using System;

namespace GodotRx.Internal
{
  internal class InstanceTracker : Godot.Object
  {
    public static readonly string OnFreedMethod = nameof(OnTrackerFreed);

    public event Action? Freed;
    
    private int _id;

    public InstanceTracker() {}

    public InstanceTracker(Godot.Object target)
    {
      _id = Singleton.RegisterInstanceTracker(this, target);
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