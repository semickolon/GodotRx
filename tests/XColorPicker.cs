using Godot;
using System;
using System.Reactive.Linq;
using GodotRx;
using GodotRx.Nodes;

public class XColorPicker : ReactiveColorPicker
{
  public override void _Ready()
  {
    base._Ready();
    
    Color
      .Throttle(TimeSpan.FromSeconds(0.2), TimeBasedScheduler.Inherit)
      .Skip(1)
      .Subscribe(color => GD.Print(color))
      .DisposeWith(this);
  }
}
