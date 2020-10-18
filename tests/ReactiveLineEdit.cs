using Godot;
using System;
using GodotRx;

namespace Tests
{
  public class ReactiveLineEdit : LineEdit
  {
    public new readonly ReactiveProperty<string> Text;

    public ReactiveLineEdit()
    {
      Text = ReactiveProperty.FromGetSet(
        () => base.Text,
        value => {
          if (base.Text != value)
          {
            base.Text = value;
          }
        }
      );
    }

    public override void _Ready()
    {
      this.OnTextChanged()
        .Subscribe(text => Text.Value = text)
        .DisposeWith(this);
    }

    public override bool _Set(string property, object value)
    {
      if (property == "text")
      {
        Text.Value = (string) value;
        return true;
      }

      return false;
    }
  }
}