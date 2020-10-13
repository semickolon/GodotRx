using Godot;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Object = Godot.Object;

namespace GodotRx
{
  internal abstract class BaseEventTracker<T> : Object
  {
    public readonly string TargetMethod = nameof(OnNext);

    private readonly Subject<object> _subject = new Subject<object>();

    public IObservable<T> OnSignal => _subject.Select(Cast).AsObservable();

    protected abstract T Cast(object o);

    private void OnNext()
      => _subject.OnNext(new Unit());

    private void OnNext(object o1)
      => _subject.OnNext(o1);

    private void OnNext(object o1, object o2)
      => _subject.OnNext((o1, o2));

    private void OnNext(object o1, object o2, object o3)
      => _subject.OnNext((o1, o2, o3));

    private void OnNext(object o1, object o2, object o3, object o4)
      => _subject.OnNext((o1, o2, o3, o4));

    private void OnNext(object o1, object o2, object o3, object o4, object o5)
      => _subject.OnNext((o1, o2, o3, o4, o5));

    private void OnNext(object o1, object o2, object o3, object o4, object o5, object o6)
      => _subject.OnNext((o1, o2, o3, o4, o5, o6));

    protected override void Dispose(bool disposing)
    {
      // GD.Print("EventTracker disposed");
      _subject.OnCompleted();
      _subject.Dispose();
      base.Dispose(disposing);
    }
  }

  internal class EventTracker : BaseEventTracker<Unit>
  {
    protected override Unit Cast(object o) => new Unit();
  }

  internal class EventTracker<T> : BaseEventTracker<T>
  {
    protected override T Cast(object o) => (T) o;
  }

  internal class EventTracker<T1, T2> : BaseEventTracker<(T1, T2)>
  {
    protected override (T1, T2) Cast(object o) 
      => ((T1, T2)) ((object, object)) o;
  }

  internal class EventTracker<T1, T2, T3> : BaseEventTracker<(T1, T2, T3)>
  {
    protected override (T1, T2, T3) Cast(object o)
      => ((T1, T2, T3)) ((object, object, object)) o;
  }

  internal class EventTracker<T1, T2, T3, T4> : BaseEventTracker<(T1, T2, T3, T4)>
  {
    protected override (T1, T2, T3, T4) Cast(object o)
      => ((T1, T2, T3, T4)) ((object, object, object, object)) o;
  }

  internal class EventTracker<T1, T2, T3, T4, T5> : BaseEventTracker<(T1, T2, T3, T4, T5)>
  {
    protected override (T1, T2, T3, T4, T5) Cast(object o)
      => ((T1, T2, T3, T4, T5)) ((object, object, object, object, object)) o;
  }

  internal class EventTracker<T1, T2, T3, T4, T5, T6> : BaseEventTracker<(T1, T2, T3, T4, T5, T6)>
  {
    protected override (T1, T2, T3, T4, T5, T6) Cast(object o)
      => ((T1, T2, T3, T4, T5, T6)) ((object, object, object, object, object, object)) o;
  }
}