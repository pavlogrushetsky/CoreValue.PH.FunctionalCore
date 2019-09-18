using System;

namespace FunctionalCore
{
    #region Discriminated Unions

    public abstract class Union2<T1, T2>
    {
        public abstract T Match<T>(Func<T1, T> f1, Func<T2, T> f2);
        public abstract void Match(Action<T1> f1, Action<T2> f2);

        private Union2() { }

        public sealed class Case1 : Union2<T1, T2>
        {
            public readonly T1 Item;
            internal Case1(T1 item) => Item = item;
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2) => f1 != null ? f1(Item) : default;
            public override void Match(Action<T1> f1, Action<T2> f2) => f1?.Invoke(Item);
        }

        public sealed class Case2 : Union2<T1, T2>
        {
            public readonly T2 Item;
            internal Case2(T2 item) => Item = item;
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2) => f2 != null ? f2(Item) : default;
            public override void Match(Action<T1> f1, Action<T2> f2) => f2?.Invoke(Item);
        }

        public static Union2<T1, T2> Case(T1 value) => new Case1(value);
        public static Union2<T1, T2> Case(T2 value) => new Case2(value);
    }

    public abstract class Union3<T1, T2, T3>
    {
        public abstract T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3);
        public abstract void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3);

        private Union3() { }

        public sealed class Case1 : Union3<T1, T2, T3>
        {
            public readonly T1 Item;
            internal Case1(T1 item) => Item = item;
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) => f1 != null ? f1(Item) : default;
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) => f1?.Invoke(Item);
        }

        public sealed class Case2 : Union3<T1, T2, T3>
        {
            public readonly T2 Item;
            internal Case2(T2 item) => Item = item;
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) => f2 != null ? f2(Item) : default;
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) => f2?.Invoke(Item);
        }

        public sealed class Case3 : Union3<T1, T2, T3>
        {
            public readonly T3 Item;
            internal Case3(T3 item) => Item = item;
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) => f3 != null ? f3(Item) : default;
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) => f3?.Invoke(Item);
        }

        public static Union3<T1, T2, T3> Case(T1 value) => new Case1(value);
        public static Union3<T1, T2, T3> Case(T2 value) => new Case2(value);
        public static Union3<T1, T2, T3> Case(T3 value) => new Case3(value);
    }

    #endregion

    #region Option

    public abstract class Option<TValue>
    {
        public abstract T Match<T>(Func<TValue, T> f1, Func<T> f2);
        public abstract void Match(Action<TValue> f1, Action f2);

        private Option() { }

        public sealed class Some : Option<TValue>
        {
            public readonly TValue Value;
            internal Some(TValue value) => Value = value;
            public override T Match<T>(Func<TValue, T> f1, Func<T> f2) => f1 != null ? f1(Value) : default;
            public override void Match(Action<TValue> f1, Action f2) => f1?.Invoke(Value);
        }

        public sealed class None : Option<TValue>
        {
            internal None() { }
            public override T Match<T>(Func<TValue, T> f1, Func<T> f2) => f2 != null ? f2() : default;
            public override void Match(Action<TValue> f1, Action f2) => f2?.Invoke();
        }
    }

    public static class Some
    {
        public static Option<T> Of<T>(T value) => new Option<T>.Some(value);
        public static Option<T> Of<T>(T? value) where T : struct =>
            value.HasValue
                ? Of(value.Value)
                : Option.None<T>();
    }

    public static class Option
    {
        public static Option<T> None<T>() => new Option<T>.None();

        public static Option<TRes> Map<T, TRes>(this Option<T> option, Func<T, TRes> map) =>
            option.Match(
                some => Some.Of(map(some)),
                None<TRes>);

        public static TRes Fold<T, TRes>(this Option<T> option, Func<T, TRes> map, TRes defaultArg) =>
            option.Match(map, () => defaultArg);
    }

    #endregion

    #region Result

    public abstract class Result<TSuccess, TError>
    {
        public abstract T Match<T>(Func<TSuccess, T> f1, Func<TError, T> f2);
        public abstract void Match(Action<TSuccess> f1, Action<TError> f2);

        private Result() { }

        public sealed class Success : Result<TSuccess, TError>
        {
            public readonly TSuccess Item;
            internal Success(TSuccess item) => Item = item;
            public override T Match<T>(Func<TSuccess, T> f1, Func<TError, T> f2) => f1 != null ? f1(Item) : default;
            public override void Match(Action<TSuccess> f1, Action<TError> f2) => f1?.Invoke(Item);
        }

        public sealed class Error : Result<TSuccess, TError>
        {
            public readonly TError Item;
            internal Error(TError item) => Item = item;
            public override T Match<T>(Func<TSuccess, T> f1, Func<TError, T> f2) => f2 != null ? f2(Item) : default;
            public override void Match(Action<TSuccess> f1, Action<TError> f2) => f2?.Invoke(Item);
        }
    }

    public static class Result
    {
        public static Result<TSuccess, TError> Success<TSuccess, TError>(TSuccess value) =>
            new Result<TSuccess, TError>.Success(value);

        public static Result<TSuccess, TError> Error<TSuccess, TError>(TError value) =>
            new Result<TSuccess, TError>.Error(value);

        public static Result<T2, TError> Bind<T1, T2, TError>(this Result<T1, TError> input, Func<T1, Result<T2, TError>> switchFunc) =>
            input.Match(switchFunc, Error<T2, TError>);

        public static Result<T, TError> Switch<T, TError>(this T input) =>
            Success<T, TError>(input);

        public static Result<T2, TError> Switch<T1, T2, TError>(this T1 input, Func<T1, T2> mapFunc) =>
            Success<T2, TError>(mapFunc(input));

        public static Result<T, TError> Switch<T, TError>(this T input, Func<T, T> mapFunc) =>
            Success<T, TError>(mapFunc(input));
    }

    #endregion
}