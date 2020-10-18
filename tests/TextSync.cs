using Godot;
using System;
using GodotRx;
using System.Reactive.Linq;

namespace Tests
{
  public class TextSync : Control
  {
    private Label? label;
    private ReactiveLineEdit? lineEdit;

    public override void _Ready()
    {
      label = GetNode<Label>("Label");
      lineEdit = GetNode<ReactiveLineEdit>("LineEdit");

      lineEdit.Text
        .Subscribe(x => label.Text = x)
        .DisposeWith(this);

      lineEdit.Text
        .Where(x => x == "bye")
        .Take(1)
        .Delay(TimeSpan.FromSeconds(0.2), TimeBasedScheduler.Inherit)
        .Subscribe(_ => QueueFree())
        .DisposeWith(this);
    }
  }
}