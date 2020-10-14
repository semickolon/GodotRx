using Godot;
using System;
using System.Reactive.Linq;

namespace GodotRx.Nodes
{
  public class ReactiveButton : Button
  {
    public new ReactiveProperty<bool> Pressed;

    public ReactiveButton()
    {
      Pressed = new ReactiveProperty<bool>(base.Pressed);
    }

    public override void _Ready()
    {
      Pressed
        .Where(x => x != base.Pressed)
        .Subscribe(x => base.Pressed = x)
        .DisposeWith(this);

      this.OnToggled()
        .Subscribe(x => Pressed.Value = x)
        .DisposeWith(this);
    }

    protected override void Dispose(bool disposing)
    {
      Pressed.DisposeWith(this);
    }
  }
}
