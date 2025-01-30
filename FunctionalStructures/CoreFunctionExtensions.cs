using System.Collections.Immutable;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public static class CoreFunctionExtensions
{
    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> srcT, Func<T, Option<R>> f)
    {
        return srcT.Bind(t => f(t).AsEnumerable());
    }

    public static Either<L, Unit> Map<L, R>(this Either<L, R> @either, Action<R> f)
    {
        return @either.Map(f.ToFunc());
    }
    
    public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> src, Action<T> f)
    {
        return src.Map(f.ToFunc()).ToImmutableList();
    }

}

public static class ActionExtensions
{
    public static Func<Unit> ToFunc(this Action action)
    {
        return () =>
        {
            action();
            return unit;
        };
    }

    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
    {
        return t =>
        {
            action(t);
            return unit;
        };
    }
}