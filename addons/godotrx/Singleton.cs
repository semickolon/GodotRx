using Godot;
using System;

using Object = Godot.Object;

namespace GodotRx.Internal
{
  internal class Singleton : Node
  {
    private static GDScript _instanceScript = (GDScript) GD.Load("res://addons/godotrx/GodotRx.gd");
    private static Node _instance = (Node) _instanceScript.New();

    public static int InjectInstanceTracker(Object target)
    {
      return (int) _instance.Call("inject_instance_tracker", target);
    }

    public static void ConnectInstanceTrackerFreed(Object target, string method)
    {
      _instance.Connect("instance_tracker_freed", target, method);
    }
  }
}