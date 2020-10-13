using Godot;
using System;

public class ObjectCounter : Label
{
  public override void _Process(float delta)
  {
    var objCount = Performance.GetMonitor(Performance.Monitor.ObjectCount);
    Text = $"Object count: {objCount}";
  }
}
