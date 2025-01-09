using static FunctionalStructures.FLib.FLibHelper;

namespace FunctionalStructures.FLib;


public struct NoneType { }

public readonly struct Option<T>
{
    private readonly T? _value;
    private readonly bool _isSome;

    internal Option(T value) 
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _isSome = true;
    }

    public static implicit operator Option<T>(NoneType _) => default;
    public static implicit operator Option<T>(T value) => value is null? None: Some(value);

    public R Match<R>(Func<T, R> some, Func<R> none) => _isSome ? some(_value!) : none();
    public Unit Match(Action<T> some, Action none) => Match(some.ToFunc(), none.ToFunc());
}


public static class FLibHelper
{
    public static NoneType None => default;

    public static Option<T> Some<T>(T value) => new(value);
    
    public static Unit Unit() => default;
}

public static class ActionExtensions
{
    public static Func<Unit> ToFunc(this Action action)
    {
        return () =>
        {
            action();
            return Unit();
        };
    }

    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
    {
        return t =>
        {
            action(t);
            return Unit();
        };
    }
}