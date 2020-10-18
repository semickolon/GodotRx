# GodotRx - Reactive Extensions for Godot C#
GodotRx is a set of reactive extensions for Godot C#, currently with signals, lifecycle events, input events, and frame events as observables, along with utility awaitable tasks, time scale-based schedulers, and reactive properties.

## Status
**Experimental.** Not yet battle-tested. API changes are likely.
We'll incorporate this to a production project, so it will probably improve over time.

## Motivation
Right now, connecting to Godot signals in C# is very similar to how you'd do it in GDScript. Though you can use idiomatic C# events for your own code, interfacing with Godot's built-in nodes, which is inevitable, would require you to write something like:
``` csharp
button.Connect("pressed", self, nameof(OnPressed));
colorPicker.Connect("color_changed", self, nameof(OnColorChanged));
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
3. Run `codegen.js` to generate the code for `SignalExtensions.cs`.

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
  
  // Singleton signals are exposed in <Singleton>Signals
  AudioServerSignals.OnBusLayoutChanged()
    .Subscribe(_ => DoAudioMagic())
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

### Input events
GodotRx offers some common input events as observables. With the tools given, you could probably make a custom observable easily if it's not readily available yet.
``` csharp
using GodotRx;

public override void _Ready()
{
  this.OnMouseDown()
    .SelectMany(_ => this.OnProcess())
    .TakeUntil(this.OnMouseUp())
    .Select(_ => GetGlobalMousePosition())
    .Subscribe(x => GD.Print(x))
    .DisposeWith(this);
  
  this.OnKeyJustPressed(KeyList.Space)
    .Subscribe(_ => player.Jump())
    .DisposeWith(this);
}
```

### Frame events
Idle, physics, pre-draw, post-draw frames are exposed as observables and awaitable tasks.
``` csharp
using GodotRx;

public override async void _Ready()
{
  this.OnIdleFrame()
    .Subscribe(delta => DoStuff(delta))
    .DisposeWith(this);
  
  this.OnFramePostDraw()
    .Subscribe(_ => GD.Print("FramePostDraw!"))
    .DisposeWith(this);
  
  await this.WaitNextPhysicsFrame();
  GD.Print("Hello");
}
```

### Delay
Internally, `WaitFor` and `WaitForSeconds` create a `SceneTreeTimer` whose timeout is exposed as an awaitable task.
``` csharp
using GodotRx;

public override async void _Ready()
{
  await this.WaitForSeconds(5);
  GD.Print("5 seconds have passed");

  await this.WaitFor(TimeSpan.FromMinutes(1));
  GD.Print("1 minute has passed");

  // By default, pauseModeProcess is true, which means the timer runs even when paused
  // If false, however, the delay timer will respect the pause state
  await this.WaitForSeconds(5, false);
  GD.Print("5 seconds (with the game unpaused) have passed");
}
```

### Time-based operations
When using time-based operations such as `Interval` or `Throttle`, make sure to use the right scheduler to achieve expected behavior.
``` csharp
using GodotRx;

public override void _Ready()
{
  Engine.TimeScale = 2.0f;
  var oneSec = TimeSpan.FromSeconds(1);

  // This respects neither the time scale nor the pause state
  // "Default" is printed every 1 second regardless of time scale
  // This keeps running regardless of pause state
  Observable.Interval(oneSec)
    .Subscribe(_ => GD.Print("Default"));

  // This respects the time scale but not the pause state
  // "Process" is printed every 0.5 seconds, since time scale is 2.0
  // This keeps running regardless of pause state
  Observable.Interval(oneSec, TimeBasedScheduler.Process)
    .Subscribe(_ => GD.Print("Process"));

  // This respects both the time scale and the pause state
  // "Inherit" is printed every 0.5 seconds, unless paused
  Observable.Interval(oneSec, TimeBasedScheduler.Inherit)
    .Subscribe(_ => GD.Print("Inherit"));
}
```

### Reactive properties
Reactive properties are lightweight property brokers, making change notifications easier.
``` csharp
public class Player : Node
{
  public ReactiveProperty<int> Hp { get; private set; }
  public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }

  public Player()
  {
    Hp = new ReactiveProperty<int>(100);
    IsDead = ReactiveProperty.Derived(Hp, hp => hp <= 0);
  }

  private void ReceiveDamage(int damage)
  {
    Hp.Value -= damage;
  }
}

public class HealthBar : Label
{
  public override void _Ready()
  {
    player.Hp
      .Subscribe(hp => Text = $"{hp} / 100")
      .DisposeWith(this);
  }
}

public class GameOverScreen : Panel
{
  public override void _Ready()
  {
    player.IsDead
      .Where(dead => dead == true)
      .Subscribe(_ => ShowOverlay())
      .DisposeWith(this);
  }
}
```

`ReactiveProperty.Computed` creates a computed property (`ReadOnlyReactiveProperty`) out of multiple reactive properties. When any of the reactive properties change value, the computed property follows suit and updates.
``` csharp
var a = new ReactiveProperty<int>(10);
var b = new ReactiveProperty<int>(20);
var sum = ReactiveProperty.Computed(a, b, (v1, v2) => v1 + v2);
```

`ReactiveProperty.FromMember` creates a reactive property out of an existing member of an instance. Setting the value of such a reactive property will reflect on the member. However, setting the member directly will not reflect on the reactive property.
``` csharp
var person = new Person("John", "Doe");
var firstName = ReactiveProperty.FromMember(person, x => x.FirstName);

firstName.Value = "Jane"
GD.Print(person.FirstName == "Jane") // True

person.FirstName = "Joe"
GD.Print(firstName.Value == "Joe") // False
```

`ReactiveProperty.FromGetSet` creates a reactive property out of a getter function and a setter function.
``` csharp
var person = new Person("John", "Doe");
var fullName = ReactiveProperty.FromGetSet(
  () => $"{person.FirstName} ${person.LastName}",
  value =>
  {
    var split = value.Split(" ");
    person.FirstName = split[0];
    person.LastName = split[1];
  }
)
```

With these tools, you can make two-way binding possible in nodes. The following example exposes the `Text` property of `LineEdit` as a reactive property that synchronizes to user changes and editor changes.
``` csharp
public class ReactiveLineEdit : LineEdit
{
  public new readonly ReactiveProperty<string> Text;

  public ReactiveLineEdit()
  {
    Text = ReactiveProperty.FromGetSet(
      () => base.Text,
      value => {
        if (base.Text != value)
          base.Text = value;
      }
    );
  }

  public override void _Ready()
  {
    // Occurs when text is changed by user
    this.OnTextChanged()
      .Subscribe(text => Text.Value = text)
      .DisposeWith(this);
  }

  public override bool _Set(string property, object value)
  {
    // Occurs when text is changed from the editor
    if (property == "text")
    {
      Text.Value = (string) value;
      return true;
    }

    return false;
  }
}
```
It's worth noting that changes in Godot objects and nodes are not fully interceptible. When an internal change happens in the engine, unless there's a signal to get notified of such a change, there is no way of detecting it. As such, the dream of making Godot fully reactive is impossible, for now.

## How it Works
### Signals
Code generation, basically. It turns out you can extract information from offline docs, including signals, from `ClassDB`.

For automatic housekeeping, when signal sources (and receivers if `DisposeWith` is used) are freed, observers are disposed. I was able to intercept an object being freed by injecting a tracker with `set_meta`. There'd only be one reference to the tracker, which is the object holding it as meta. Now, once the object is freed, there'll be no more references pointing to the tracker, emitting a predelete notification (interception point), then finally freeing it.

For some reason, this hack only works in GDScript. The tracker isn't automatically freed when done in C#, so I did it all in GDScript with some C# wrapper code.

Finally, I could've used and added a child node which also gets freed once its parent is freed, but that only works for nodes. This hacky approach works for all Godot objects.

### Node lifecycle
It seems like there's absolutely no way to intercept lifecycle method calls, so when you use lifecycle observables, a child node is added (if there's not one yet) which is the one doing the intercepting, albeit indirectly. As such, if you must absolutely rely on node count and order for making things work, you might have to rethink your approach.

### Time-based operations
All thanks to `SceneTree.CreateTimer()`, lightweight timers can effortlessly be made in a way that respects `Engine.TimeScale` and optionally `SceneTree.Paused`.

## License
Distributed under the [MIT License](https://github.com/semickolon/GodotRx/blob/master/LICENSE).