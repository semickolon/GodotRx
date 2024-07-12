using Godot;
using System;
using GodotRx;

namespace Tests
{
  public partial class PauseButton : Button
  {
    public override void _Ready()
    {
      this.OnToggled()
        .Subscribe(paused => GetTree().Paused = paused);
    }
  }
}