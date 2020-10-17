using Godot;
using System;
using GodotRx;
using System.Reactive.Linq;

public class Area2D : Godot.Area2D
{
  public override async void _Ready()
  {
    this.OnBodyShapeEntered()
      .Subscribe(t => GD.Print("Body entered: ", t.Body, t.BodyId))
      .DisposeWith(this);

    Engine.TimeScale = 0.5f;

    Observable
      .Interval(TimeSpan.FromSeconds(1), TimeBasedScheduler.Inherit)
      .Subscribe(_ => GD.Print("1s repeating! (INHERIT)"));

    Observable
      .Interval(TimeSpan.FromSeconds(1), TimeBasedScheduler.Process)
      .Subscribe(_ => GD.Print("1s repeating! (PROCESS)"));

    Observable
      .Interval(TimeSpan.FromSeconds(1))
      .Subscribe(_ => GD.Print("1s repeating! (DEFAULT)"));
    
    await this.WaitForSeconds(2, false);
    GD.Print("2s!");
  }
}
