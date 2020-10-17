using Godot;
using System;
using GodotRx;
using System.Reactive.Linq;

public class Area2D : Godot.Area2D
{
  public override void _Ready()
  {
    this.OnBodyShapeEntered()
      .Subscribe(t => GD.Print("Body entered: ", t.Body, t.BodyId))
      .DisposeWith(this);

    Engine.TimeScale = 0.5f;

    Observable
      .Timer(TimeSpan.FromSeconds(2), TimeBasedScheduler.Instance)
      .Subscribe(_ => GD.Print("GOTEM"));
  }
}
