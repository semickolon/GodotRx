using Godot;
using System;
using GodotRx;
using GodotRx.Nodes;
using System.Reactive.Linq;

public class TextSync : Control
{
  Label? label;
  ReactiveLineEdit? lineEdit;

  public override void _Ready()
  {
    label = GetNode<Label>("Label");
    lineEdit = GetNode<ReactiveLineEdit>("LineEdit");

    lineEdit.Text
      .Subscribe(x => label.Text = x)
      .DisposeWith(this);
  }
}
