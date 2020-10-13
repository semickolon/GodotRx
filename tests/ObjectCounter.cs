using Godot;
using System;
using System.Reactive.Linq;
using GodotRx;

public class ObjectCounter : Label
{
  public override void _Ready()
  {
    this.OnProcess().Subscribe(_ =>
    {
      var objCount = Performance.GetMonitor(Performance.Monitor.ObjectCount);
      Text = $"Object count: {objCount}";
    });

    this.OnProcess()
      .CombineLatest(this.OnPhysicsProcess(), (a, b) => (a, b))
      .Sample(new TimeSpan(0, 0, 1))
      .Subscribe(t => GD.Print($"Process: {t.a} | Physics process: {t.b}"));

    this.OnActionJustPressed("ui_left")
      .Subscribe(_ => GD.Print("trigger"));
  }
}
