using Godot;

using Object = Godot.Object;

namespace GodotRx.Internal
{
  internal class Singleton
  {
    private static GDScript _instanceScript = (GDScript) GD.Load("res://addons/godotrx/GodotRx.gd");
    private static Object _instance = (Object) _instanceScript.New();

    public static int RegisterInstanceTracker(InstanceTracker tracker, Object target)
    {
      var id = (int) _instance.Call("inject_instance_tracker", target);
      _instance.Connect("instance_tracker_freed", tracker, InstanceTracker.OnFreedMethod);
      return id;
    }
  }
}