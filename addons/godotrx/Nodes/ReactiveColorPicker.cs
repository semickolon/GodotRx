using Godot;
using System;
using System.Reactive.Linq;

namespace GodotRx.Nodes
{
  public class ReactiveColorPicker : Godot.ColorPicker
  {
    public new ReactiveProperty<Color> Color;

    public ReactiveColorPicker()
    {
      Color = new ReactiveProperty<Color>(base.Color);
    }

    public override void _Ready()
    {
      Color
        .Where(x => x != base.Color)
        .Subscribe(x => base.Color = x)
        .DisposeWith(this);

      this.OnColorChanged()
        .Subscribe(x => Color.Value = x)
        .DisposeWith(this);
    }

    protected override void Dispose(bool disposing)
    {
      Color.DisposeWith(this);
    }
  }
}
