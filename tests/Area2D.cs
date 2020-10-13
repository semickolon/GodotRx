using Godot;
using System;
using GodotRx;

public class Area2D : Godot.Area2D
{
  public override void _Ready()
  {
    this.OnBodyShapeEntered()
      .Subscribe(t => GD.Print("Body entered: ", t.Body, t.BodyId));
  }
}
