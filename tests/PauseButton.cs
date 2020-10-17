using Godot;
using System;
using GodotRx;
using GodotRx.Nodes;

public class PauseButton : ReactiveButton
{
  public override void _Ready()
  {
    this.OnToggled()
      .Subscribe(paused => GetTree().Paused = paused);
  }
}
