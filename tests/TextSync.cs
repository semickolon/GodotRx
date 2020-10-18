using Godot;
using System;
using GodotRx;
using System.Reactive.Linq;

public class TextSync : Control
{
  Label? label;
  LineEdit? lineEdit;

  public override void _Ready()
  {
    label = GetNode<Label>("Label");
    lineEdit = GetNode<LineEdit>("LineEdit");

    lineEdit.OnTextChanged()
      .Subscribe(x => label.Text = x)
      .DisposeWith(this);
    
    lineEdit.OnTextChanged()
      .Where(x => x == "bye")
      .Take(1)
      .Delay(TimeSpan.FromSeconds(0.2), TimeBasedScheduler.Inherit)
      .Subscribe(_ => QueueFree())
      .DisposeWith(this);
  }
}
