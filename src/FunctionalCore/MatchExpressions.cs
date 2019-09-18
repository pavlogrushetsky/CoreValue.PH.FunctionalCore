using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FunctionalCore
{
    public interface ICloneable<out T>
    {
        T Clone();
    }

    public sealed class MatchExpression<TArg>
    {
        internal TArg Arg { get; }

        internal MatchExpression(TArg arg) =>
            Arg = arg;

        public WithExpression<TArg, TRes> With<TRes>(TArg arg, Func<TArg, TRes> match) =>
            new WithExpression<TArg, TRes>(Arg, new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case1(new[] { arg }), match, null));

        public WithExpression<TArg, TRes> With<TRes>(Func<TArg, bool> arg, Func<TArg, TRes> match) =>
            new WithExpression<TArg, TRes>(Arg, new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case2(arg), match, null));

        public WithExpression<TArg, TRes> With<TRes>(TArg[] arg, Func<TArg, TRes> match) =>
            new WithExpression<TArg, TRes>(Arg, new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case1(arg), match, null));

        public WithExpression<TArg, TRes> WithWhen<TRes>(TArg arg, Func<TArg, TRes> match, Func<bool> when) =>
            new WithExpression<TArg, TRes>(Arg, new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case1(new[] { arg }), match, when));

        public WithExpression<TArg, TRes> WithWhen<TRes>(Func<TArg, bool> arg, Func<TArg, TRes> match, Func<bool> when) =>
            new WithExpression<TArg, TRes>(Arg, new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case2(arg), match, when));
    }

    public sealed class WithExpression<TArg, TRes>
    {
        internal TArg Arg { get; }

        private readonly ImmutableList<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> _expr;

        internal IEnumerable<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> Expr => _expr;

        private WithExpression(TArg arg, IEnumerable<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> expr)
        {
            Arg = arg;
            _expr = expr.ToImmutableList();
        }

        internal WithExpression(TArg arg, Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>> expr)
            : this(arg, new List<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> { expr }) { }

        public WithExpression<TArg, TRes> With(TArg arg, Func<TArg, TRes> match) =>
            new WithExpression<TArg, TRes>(Arg, _expr.Add(new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case1(new[] { arg }), match, null)));

        public WithExpression<TArg, TRes> With(Func<TArg, bool> arg, Func<TArg, TRes> match) =>
            new WithExpression<TArg, TRes>(Arg, _expr.Add(new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case2(arg), match, null)));

        public WithExpression<TArg, TRes> WithWhen(TArg arg, Func<TArg, TRes> match, Func<bool> when) =>
            new WithExpression<TArg, TRes>(Arg, _expr.Add(new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case1(new[] { arg }), match, when)));

        public WithExpression<TArg, TRes> WithWhen(Func<TArg, bool> arg, Func<TArg, TRes> match, Func<bool> when) =>
            new WithExpression<TArg, TRes>(Arg, _expr.Add(new Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>(new Union2<TArg[], Func<TArg, bool>>.Case2(arg), match, when)));

        public WithDefaultExpression<TArg, TRes> WithDefault(Func<TArg, TRes> match) =>
            new WithDefaultExpression<TArg, TRes>(Arg, Expr, match);
    }

    public sealed class WithDefaultExpression<TArg, TRes>
    {
        internal TArg Arg { get; }

        private readonly ImmutableList<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> _expr;

        internal IEnumerable<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> Expr => _expr;

        internal Func<TArg, TRes> DefaultExpr { get; }

        internal WithDefaultExpression(TArg arg, IEnumerable<Tuple<Union2<TArg[], Func<TArg, bool>>, Func<TArg, TRes>, Func<bool>>> expr, Func<TArg, TRes> defaultExpr)
        {
            Arg = arg;
            _expr = expr.ToImmutableList();
            DefaultExpr = defaultExpr;
        }

        public TRes Evaluate()
        {
            if (!Expr.Any())
                return default;

            foreach (var tuple in Expr.Where(tuple => tuple.Item1.Match(
                                                          f1 => f1.Any(v => Arg.Equals(v)),
                                                          f2 => f2(Arg))
                                                      && (tuple.Item3 == null || tuple.Item3())))
                return tuple.Item2(Arg);

            return DefaultExpr == null ? default : DefaultExpr(Arg);
        }
    }

    public static class MatchExpressionFunctions
    {
        public static MatchExpression<TArg> Match<TArg>(this TArg arg) =>
            new MatchExpression<TArg>(arg);

        public static MatchExpression<TArg> MatchClone<TArg>(this ICloneable<TArg> arg) =>
            new MatchExpression<TArg>(arg.Clone());
    }
}