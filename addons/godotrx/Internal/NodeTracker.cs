using Godot;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GodotRx.Internal
{
  internal class NodeTracker : Node
  {
    public static readonly string DefaultName = "__NodeTracker__";

    private Subject<float>? _onProcess;
    private Subject<float>? _onPhysicsProcess;
    private Subject<InputEvent>? _onInput;
    private Subject<InputEvent>? _onUnhandledInput;
    private Subject<InputEventKey>? _onUnhandledKeyInput;

    public IObservable<float> OnProcess
    {
      get
      {
        if (_onProcess == null)
        {
          _onProcess = new Subject<float>();
          SetProcess(true);
        }

        return _onProcess.AsObservable();
      }
    }

    public IObservable<float> OnPhysicsProcess
    {
      get
      {
        if (_onPhysicsProcess == null)
        {
          _onPhysicsProcess = new Subject<float>();
          SetPhysicsProcess(true);
        }

        return _onPhysicsProcess.AsObservable();
      }
    }

    public IObservable<InputEvent> OnInput
    {
      get
      {
        if (_onInput == null)
        {
          _onInput = new Subject<InputEvent>();
          SetProcessInput(true);
        }

        return _onInput.AsObservable();
      }
    }

    public IObservable<InputEvent> OnUnhandledInput
    {
      get
      {
        if (_onUnhandledInput == null)
        {
          _onUnhandledInput = new Subject<InputEvent>();
          SetProcessUnhandledInput(true);
        }

        return _onUnhandledInput.AsObservable();
      }
    }

    public IObservable<InputEventKey> OnUnhandledKeyInput
    {
      get
      {
        if (_onUnhandledKeyInput == null)
        {
          _onUnhandledKeyInput = new Subject<InputEventKey>();
          SetProcessUnhandledKeyInput(true);
        }

        return _onUnhandledKeyInput.AsObservable();
      }
    }

    public override void _Ready()
    {
      SetProcess(false);
      SetPhysicsProcess(false);
      SetProcessInput(false);
      SetProcessUnhandledInput(false);
      SetProcessUnhandledKeyInput(false);
    }

    public override void _Process(float delta)
    {
      _onProcess?.OnNext(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
      _onPhysicsProcess?.OnNext(delta);
    }

    public override void _Input(InputEvent ev)
    {
      _onInput?.OnNext(ev);
    }

    public override void _UnhandledInput(InputEvent ev)
    {
      _onUnhandledInput?.OnNext(ev);
    }

    public override void _UnhandledKeyInput(InputEventKey ev)
    {
      _onUnhandledKeyInput?.OnNext(ev);
    }

    protected override void Dispose(bool disposing)
    {
      _onProcess?.CompleteAndDispose();
      _onPhysicsProcess?.CompleteAndDispose();
      _onInput?.CompleteAndDispose();
      _onUnhandledInput?.CompleteAndDispose();
      _onUnhandledKeyInput?.CompleteAndDispose();

      base.Dispose(disposing);
    }
  }
}