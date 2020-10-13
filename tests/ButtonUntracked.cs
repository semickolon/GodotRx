using Godot;
using System;

public class ButtonUntracked : Godot.Button
{
  public override void _Ready()
  {
    this.Connect("pressed", this, "queue_free");
  }
}
