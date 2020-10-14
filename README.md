# GodotRx - Reactive Extensions for Godot C#
GodotRx is a set of reactive extensions for Godot C#, currently with signals, lifecycle methods, and common input events as observables.

## Status
**Experimental.** We'll incorporate this to a production project, so it will probably improve over time.

## Motivation
Right now, connecting to Godot signals in C# is very similar to how you'd do it in GDScript. Though you can use idiomatic C# events for your own code, interfacing with Godot's built-in nodes, which is inevitable, would require you to write something like:
``` csharp
button.Connect("pressed", self, nameof(OnPressed));
colorPicker.Connect("color_changed", self, nameof(OnColorChanged))
```
For someone looking for a more production-safe environment, there are alarming concerns right away.

1. Signal names are hardcoded as strings.
2. No type information for methods. What information, if any, will I receive?
3. Only class methods can be directly connected. How about lambda expressions?

Upon looking for existing workarounds, [AlleyCat](https://github.com/mysticfall/AlleyCat) is great work! You can hook into signals as observables, solving problem 3. However, at the time of writing, signal names are still strings and the type information is limited just to `object`. *I respect the work put into AlleyCat, and it does more than solving this problem, so I recommend checking it out!*

GodotRx aims to solve all of this by turning signals into observables, so you could write something like:
``` csharp
button.OnPressed()
  .Subscribe(_ => DoStuff())
  .DisposeWith(this);

colorPicker.OnColorChanged()
  .Subscribe(color => label.Modulate = color) // color is Godot.Color
  .DisposeWith(this);
```

[Official support for signals as events coming soon](https://godotengine.org/article/csharp-ios-signals-events)

## Installation
As far as I know, Godot 3.x lacks support for plugins written in C#. Until we get proper support, you'll have to copy and paste `addons/godotrx` into your own project, then make sure to enable the plugin. **C# 8.0+ is required. Written and tested on Godot 3.2.3 stable.**

If the generated code for signals as observables doesn't match up with your version of Godot, you might want to manually generate it as such:
1. Add a `SignalDataGen` node in any scene.
2. Run the scene, `signal_data.json` will be generated.
3. Run `codegen.js` to generate the body for `SignalExtensions.cs`.

## Usage
### Signals
Almost every signal from GDScript is mapped to C# as an extension method with type information retained. For example, the `Area` signal named `area_shape_entered` would be `OnAreaShapeEntered()`
``` csharp
using GodotRx;

public override void _Ready()
{
  // For signals with no arguments, you'd get a Unit
  // t is Unit
  button.OnPressed()
    .Subscribe(t => DoStuff())
    .DisposeWith(this);
  
  // For signals with 1 argument, you'd get that argument
  // color is Godot.Color
  colorPicker.OnColorChanged()
    .Subscribe(color => label.Modulate = color)
    .DisposeWith(this);

  // For signals with 2 or more arguments, you'd get a named ValueTuple
  // t is (int AreaId, Godot.Area Area, int AreaShape, int SelfShape)
  area.OnAreaShapeEntered()
    .Subscribe(t => GD.Print(t.Area, t.SelfShape))
    .DisposeWith(this);
}
```
Subscriptions are automatically disposed when the event source is freed. However, it's possible that the source is alive but the receiver isn't. When the source emits, the subscriber may access something already freed like `this`, causing errors.

To prevent this, use `DisposeWith(Godot.Object)` to automatically dispose the subscriber when the Godot object argument (presumably the receiver) is freed. Now, once the source or receiver is freed, whichever goes first, the subscriber is disposed. There are cases when the source and receiver are the same, but I think it's good practice nonetheless.

### Node lifecycle
Inspired by [UniRx](https://github.com/neuecc/UniRx), lifecycle methods such as `_Process` and `_Input` can be made into observables.
``` csharp
using GodotRx;

public override void _Ready()
{
  this.OnProcess()
    .Where(delta => Input.IsActionJustPressed("jump"))
    .Subscribe(delta => player.Jump(delta))
    .DisposeWith(this);
  
  this.OnUnhandledInput()
    .Where(ev => ev is InputEventKey)
    .Subscribe(DoStuff)
    .DisposeWith(this);
}
```
Similar to AlleyCat's limitations, the events you'd receive from these lifecycle observables don't come from the node itself, but from a child node automatically added acting as some kind of proxy. This is because there seems to be no way to directly intercept calls to these lifecycle methods.

Again, there is no need for manual housekeeping. When the node itself is freed, the child node proxy is freed, which then disposes observers.

### Common input events
GodotRx offers some common input events as observables. With the tools given, you could probably make a custom observable easily if it's not readily available yet.
``` csharp
using GodotRx;

public override void _Ready()
{
  this.OnMouseDown()
    .SelectMany(_ => this.OnProcess())
    .TakeUntil(this.OnMouseUp())
    .Select(_ => GetGlobalMousePosition())
    .Subscribe(x => GD.Print(x));
  
  this.OnKeyJustPressed(KeyList.Space)
    .Subscribe(_ => player.Jump())
    .DisposeWith(this);
}
```

## Progress
✅ Signal observables

✅ Lifecycle observables

✅ Input observables

🚧 More utility observables

🚧 Reactive properties

🚧 Reactive data structures

🚧 Computed properties

🚧 Two-way binding / Reactive nodes

## How it Works
### Signals
Code generation, basically. It turns out you can extract information from offline docs, including signals, from `ClassDB`. After generating, it all goes to `SignalExtensions.cs`.

For automatic housekeeping, when signal sources (and receivers if `DisposeWith` is used) are freed, observers are disposed. I was able to intercept an object being freed by injecting a tracker with `set_meta`. There'd only be one reference to the tracker, which is the object holding it as meta. Now, once the object is freed, there'll be no more references pointing to the tracker, emitting a predelete notification (interception point), then finally freeing it.

For some reason, this hack only works in GDScript. The tracker isn't automatically freed when done in C#, so I did it all in GDScript with some C# wrapper code.

Finally, I could've used and added a child node which also gets freed once its parent is freed, but that only works for nodes. This hacky approach works for all Godot objects.

### Node lifecycle
It seems like there's absolutely no way to intercept lifecycle method calls, so when you use lifecycle observables, a child node is added (if there's not one yet) which is the one doing the intercepting, albeit indirectly. As such, if you must absolutely rely on node count and order for making things work, you might have to rethink your approach.

### Common input events
Simply making use of lifecycle observables.

## License
Distributed under the [MIT License](https://github.com/semickolon/GodotRx/blob/master/LICENSE).