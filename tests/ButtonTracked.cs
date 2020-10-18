using Godot;
using System;
using GodotRx;

namespace Tests
{
  public class ButtonTracked : Button
  {
    public override void _Ready()
    {
      this.OnToggled()
        .Subscribe(_ => { })
        .DisposeWith(this);

      this.OnPressed();
      this.OnPressed();
      this.OnMinimumSizeChanged();
      this.OnMinimumSizeChanged();
      this.OnFocusEntered();
      this.OnFocusEntered();

      this.OnPressed()
        .Subscribe(_ => QueueFree())
        .DisposeWith(this);
    }
  }
}