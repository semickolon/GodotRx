using Godot;
using System;
using GodotRx;

namespace Tests
{
  public partial class ToggleButton : Button
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
      Text = ButtonPressed ? "YES" : "NO";
    }
  }
}