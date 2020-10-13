using Godot;
using System;
using GodotRx;

public class ColorPicker : Godot.ColorPicker
{
  public override void _Ready()
  {
    this.OnColorChanged().Subscribe(color => GD.Print(color));
  }
}
