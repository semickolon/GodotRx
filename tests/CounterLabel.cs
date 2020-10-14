using Godot;
using System;
using GodotRx;

public class CounterLabel : Label
{
  public override void _Ready()
  {
    var v = new Vector2i();
    var vx = ReactiveProperty.FromMember(v, v => v.x);
    var vxr = vx.ToReadOnly();
    
    vxr.Subscribe(x => Text = $"{x} ({v.x}, {v.y})");

    this.OnActionJustPressed("ui_up")
      .Subscribe(_ => vx.Value += 1)
      .DisposeWith(this);
  }

  public class Vector2i
  {
    public int x = 0;
    public int y = 0;
  }
}
