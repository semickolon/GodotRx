using Godot;
using System;
using GodotRx;

public class ToggleButton : Button
{
  public override void _Ready()
  {
    this.OnToggled()
      .Subscribe(_ => UpdateText())
      .DisposeWith(this);
    
    UpdateText();
  }

  private void UpdateText()
  {
    Text = Pressed ? "YES" : "NO";
  }
}
