using Godot;
using System;

namespace Tests
{
  public class ButtonUntracked : Button
  {
    public override void _Ready()
    {
      this.Connect("pressed", this, "queue_free");
    }
  }
}