using Godot;
using System;
using System.Reactive.Linq;

namespace GodotRx.Nodes
{
  public class ReactiveLineEdit : LineEdit
  {
    public new ReactiveProperty<string> Text;

    public ReactiveLineEdit()
    {
      Text = new ReactiveProperty<string>(base.Text);
    }

    public override void _Ready()
    {
      Text
        .Where(x => x != base.Text)
        .Subscribe(x => base.Text = x)
        .DisposeWith(this);

      this.OnTextChanged()
        .Subscribe(x => Text.Value = x)
        .DisposeWith(this);
    }

    protected override void Dispose(bool disposing)
    {
      Text.DisposeWith(this);
    }
  }
}
