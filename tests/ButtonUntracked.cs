using Godot;
using System;

namespace Tests
{
  public partial class ButtonUntracked : Button
  {
    public override void _Ready()
    {
      this.Connect("pressed", new Callable(this, "queue_free"));
    }
  }
}