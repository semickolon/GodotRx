using Godot;
using System;

namespace GodotRx
{
  internal class InstanceTracker : Godot.Object
  {
    public event Action? Freed;
    
    private int _id;

    public InstanceTracker() {}

    public InstanceTracker(Godot.Object target)
    {
      _id = Singleton.InjectInstanceTracker(target);
      Singleton.ConnectInstanceTrackerFreed(this, nameof(OnTrackerFreed));
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