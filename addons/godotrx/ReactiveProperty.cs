using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Godot;

using Object = System.Object;
using Expression = System.Linq.Expressions.Expression;

namespace GodotRx
{
  public interface IReadOnlyReactiveProperty<out T> : IObservable<T>, IDisposable
  {
    T Value { get; }
  }

  public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
  {
    new T Value { get; set; }
  }

  public class ReadOnlyReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IObserver<T>
  {
    public T Value => _latestValue;
    public bool IsDisposed { get; private set; } = false;

    private T _latestValue;
    private IDisposable _sourceSubscription;
    private List<IObserver<T>> _observers = new List<IObserver<T>>();
    private bool _distinctUntilChanged;
    private bool _raiseLatestValueOnSubscribe;

    public ReadOnlyReactiveProperty(
      IObservable<T> source,
      T initialValue,
      bool distinctUntilChanged = true,
      bool raiseLatestValueOnSubscribe = true)
    {
      _sourceSubscription = source.Subscribe(this);
      _latestValue = initialValue;
      _distinctUntilChanged = distinctUntilChanged;
      _raiseLatestValueOnSubscribe = raiseLatestValueOnSubscribe;
    }

    public void Dispose()
    {
      if (IsDisposed)
        return;
      
      IsDisposed = true;

      _observers.ForEach(observer => observer.OnCompleted());
      _observers.Clear();

      _sourceSubscription.Dispose();
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
      if (IsDisposed)
      {
        observer.OnCompleted();
        return Disposable.Empty;
      }

      _observers.Add(observer);

      if (_raiseLatestValueOnSubscribe)
      {
        observer.OnNext(_latestValue);
      }

      return Disposable.Create(() => _observers.Remove(observer));
    }

    public void OnNext(T value)
    {
      if (_distinctUntilChanged && Object.Equals(_latestValue, value))
        return;

      _latestValue = value;
      _observers.ForEach(observer => observer.OnNext(value));
    }

    public void OnError(Exception error)
    {
      _observers.ForEach(observer => observer.OnError(error));
    }

    public void OnCompleted()
    {
      Dispose();
    }
  }

  public class ReactiveProperty<T> : IReactiveProperty<T>
  {
    public T Value { 
      get => _latestValue; 
      set
      {
        if (_distinctUntilChanged && Object.Equals(_latestValue, value))
          return;

        _latestValue = value;
        _observers.ForEach(observer => observer.OnNext(value));
      }
    }

    public bool IsDisposed { get; private set; } = false;

    private T _latestValue;
    private IDisposable? _sourceSubscription = null;
    private List<IObserver<T>> _observers = new List<IObserver<T>>();
    private bool _distinctUntilChanged;
    private bool _raiseLatestValueOnSubscribe;

    public ReactiveProperty(
      T initialValue,
      bool distinctUntilChanged = true,
      bool raiseLatestValueOnSubscribe = true)
    {
      _latestValue = initialValue;
      _distinctUntilChanged = distinctUntilChanged;
      _raiseLatestValueOnSubscribe = raiseLatestValueOnSubscribe;
    }

    public ReactiveProperty(
      IObservable<T> source,
      T initialValue,
      bool distinctUntilChanged = true,
      bool raiseLatestValueOnSubscribe = true)
      : this(initialValue, distinctUntilChanged, raiseLatestValueOnSubscribe)
    {
      _sourceSubscription = source.Subscribe(x => Value = x);
    }

    public void Dispose()
    {
      if (IsDisposed)
        return;
      
      IsDisposed = true;

      _observers.ForEach(observer => observer.OnCompleted());
      _observers.Clear();
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
      if (IsDisposed)
      {
        observer.OnCompleted();
        return Disposable.Empty;
      }

      _observers.Add(observer);

      if (_raiseLatestValueOnSubscribe)
      {
        observer.OnNext(_latestValue);
      }

      return Disposable.Create(() => _observers.Remove(observer));
    }

    public ReadOnlyReactiveProperty<T> ToReadOnly()
    {
      return new ReadOnlyReactiveProperty<T>(this, Value, _distinctUntilChanged, _raiseLatestValueOnSubscribe);
    }
  }

  public static class ReactiveProperty
  {
    public static ReactiveProperty<TProp> FromMember<TTarget, TProp>(
      TTarget target,
      Expression<Func<TTarget, TProp>> memberSelector,
      bool distinctUntilChanged = true,
      bool raiseLatestValueOnSubscribe = true)
    {
      return FromMember(
        target, 
        memberSelector, 
        x => x,
        x => x,
        distinctUntilChanged,
        raiseLatestValueOnSubscribe
      );
    }

    public static ReactiveProperty<TResult> FromMember<TTarget, TProp, TResult>(
      TTarget target,
      Expression<Func<TTarget, TProp>> memberSelector,
      Func<TProp, TResult> convert,
      Func<TResult, TProp> convertBack,
      bool distinctUntilChanged = true,
      bool raiseLatestValueOnSubscribe = true)
    {
      if (!(memberSelector.Body is MemberExpression memberExpr))
      {
        throw new ArgumentException("Invalid memberSelector, not a MemberExpression");
      }
      
      var targetParam = Expression.Parameter(typeof(TTarget), "target");
      var valueParam = Expression.Parameter(typeof(TProp), "value");
      var memberAccessExpr = Expression.PropertyOrField(targetParam, memberExpr.Member.Name);
      var assignExpr = Expression.Assign(memberAccessExpr, valueParam);
      var setterExpr = Expression.Lambda<Action<TTarget, TProp>>(assignExpr, targetParam, valueParam);

      var getter = memberSelector.Compile();
      var setter = setterExpr.Compile();

      var prop = new ReactiveProperty<TResult>(
        convert(getter(target)),
        distinctUntilChanged,
        raiseLatestValueOnSubscribe
      );

      prop.Subscribe(x => {
        setter(target, convertBack(x));
        GD.Print("wow ", target);
      });

      return prop;
    }
  }
}