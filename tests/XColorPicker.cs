using Godot;
using System;
using System.Reactive.Linq;
using GodotRx;

public class XColorPicker : ColorPicker
{
  public override void _Ready()
  {
    this.OnColorChanged()
      .Throttle(TimeSpan.FromSeconds(0.2), TimeBasedScheduler.Inherit)
      .Skip(1)
      .Subscribe(color => GD.Print(color))
      .DisposeWith(this);
  }
}
