using Godot;
using System;
using GodotRx;
using GodotRx.Nodes;

public class ToggleButton : ReactiveButton
{
  public override void _Ready()
  {
    base._Ready();
    
    this.Pressed
      .Subscribe(x => Text = x ? "YES" : "NO")
      .DisposeWith(this);
  }
}
