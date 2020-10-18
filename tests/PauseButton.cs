using Godot;
using System;
using GodotRx;

public class PauseButton : Button
{
  public override void _Ready()
  {
    this.OnToggled()
      .Subscribe(paused => GetTree().Paused = paused);
  }
}
