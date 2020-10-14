using Godot;
using System;
using GodotRx;

public class ButtonTarget : Button
{
  Button? buttonSource;

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
