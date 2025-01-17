using System.Collections.Immutable;
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
    
    public IEnumerable<T> AsEnumerable()
    {
        if (_isSome)
        {
            yield return _value!;
        }
    }
}


public static class FLibHelper
{
    public static NoneType None => default;

    public static Option<T> Some<T>(T value) => new(value);
    
    public static Unit Unit() => default;
    
    public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f)
    {
        return optT.Match(
            none: () => None,
            some: t => Some(f(t)));
    }
    
    public static Option<Unit> Map<T>(this Option<T> optT, Action<T> f)
    {
        return optT.Map(f.ToFunc());
    }

    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> src, Func<T, R> f) => src.Select(f);

    public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> src, Action<T> f)
    {
        return src.Map(f.ToFunc()).ToImmutableList();
    }
    
    public static IEnumerable<Unit> Map<T>(this IEnumerable<T> src, Action<T> f)
    {
        return src.Map(f.ToFunc()).ToImmutableList();
    }

    public static Option<R> Bind<T, R>(this Option<T> optionT, Func<T, Option<R>> f)
    {
        return optionT.Match(
            none: () => None,
            some: f
        );
    }

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> srcT, Func<T, IEnumerable<R>> f)
    {
        foreach (T t in srcT)
        {
            foreach (R nested in f(t))
            {
                yield return nested;
            }
        }
    }

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> srcT, Func<T, Option<R>> f)
    {
        return srcT.Bind(t => f(t).AsEnumerable());
    }

    public static IEnumerable<R> Bind<T, R>(this Option<T> optT, Func<T, IEnumerable<R>> f)
    {
        return optT.AsEnumerable().Bind(f);
    }

    public static Option<T> Where<T>(this Option<T> optT, Func<T, bool> pred)
    {
        return optT.Match(
            some: t => pred(t) ? optT : None,
            none: () => None
        );
    }
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