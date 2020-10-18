using Godot;
using System;
using GodotRx;

namespace Tests
{
  public class ButtonTarget : Button
  {
    private Button? buttonSource;

    public override void _Ready()
    {
      buttonSource = GetNode<Button>("../ButtonSource");
      buttonSource.OnPressed()
        .Subscribe(_ => GD.Print(this))
        .DisposeWith(this);

      this.OnPressed()
        .Subscribe(_ => QueueFree())
        .DisposeWith(this);
    }
  }
}