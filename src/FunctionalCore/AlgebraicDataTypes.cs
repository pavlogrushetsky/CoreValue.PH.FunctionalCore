using System;

namespace FunctionalCore
{
    #region Discriminated Unions

    /// <summary>
    /// The discriminated union with two union cases.
    /// </summary>
    /// <typeparam name="T1">The type of the first union case.</typeparam>
    /// <typeparam name="T2">The type of the second union case.</typeparam>
    public abstract class Union2<T1, T2>
    {
        /// <summary>
        /// Match the cases of the discriminated union with the corresponding functions.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="f1">The function to match with the first union case.</param>
        /// <param name="f2">The function to match with the second union case.</param>
        /// <returns>The result value.</returns>
        public abstract T Match<T>(Func<T1, T> f1, Func<T2, T> f2);

        /// <summary>
        /// Match the cases of the discriminated union with the corresponding actions.
        /// </summary>
        /// <param name="f1">The action to match with the first union case.</param>
        /// <param name="f2">The action to match with the second union case.</param>
        public abstract void Match(Action<T1> f1, Action<T2> f2);

        private Union2() { }

        internal sealed class UnionCase1 : Union2<T1, T2>
        {
            internal readonly T1 Item;

            internal UnionCase1(T1 item) 
                => Item = item;

            /// <inheritdoc />
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2) 
                => f1 != null 
                    ? f1(Item) 
                    : default;

            /// <inheritdoc />
            public override void Match(Action<T1> f1, Action<T2> f2) 
                => f1?.Invoke(Item);
        }

        internal sealed class UnionCase2 : Union2<T1, T2>
        {
            internal readonly T2 Item;

            internal UnionCase2(T2 item) 
                => Item = item;

            /// <inheritdoc />
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2) 
                => f2 != null 
                    ? f2(Item) 
                    : default;

            /// <inheritdoc />
            public override void Match(Action<T1> f1, Action<T2> f2) 
                => f2?.Invoke(Item);
        }

        /// <summary>
        /// Construct a value of the discriminated union using the first union case.
        /// </summary>
        /// <param name="value">The value of the first union case.</param>
        /// <returns>The discriminated union value.</returns>
        public static Union2<T1, T2> Case1(T1 value) 
            => new UnionCase1(value);

        /// <summary>
        /// Construct a value of the discriminated union using the second union case.
        /// </summary>
        /// <param name="value">The value of the second union case.</param>
        /// <returns>The discriminated union value.</returns>
        public static Union2<T1, T2> Case2(T2 value) 
            => new UnionCase2(value);

        #region Equality

        public override bool Equals(object obj) 
            => obj is Union2<T1, T2> other 
               && Equals(other);

        protected bool Equals(Union2<T1, T2> other) 
            => other != null 
               && other.Match(
                   otherCase1 => Match(
                       thisCase1 => otherCase1.Equals(thisCase1), 
                       thisCase2 => false),
                   otherCase2 => Match(
                       thisCase1 => false, 
                       thisCase2 => otherCase2.Equals(thisCase2)));

        public override int GetHashCode() 
            => Match(
                case1 => case1.GetHashCode(), 
                case2 => case2.GetHashCode());

        #endregion
    }

    /// <summary>
    /// The discriminated union with three union cases.
    /// </summary>
    /// <typeparam name="T1">The type of the first union case.</typeparam>
    /// <typeparam name="T2">The type of the second union case.</typeparam>
    /// <typeparam name="T3">The type of the third union case.</typeparam>
    public abstract class Union3<T1, T2, T3>
    {
        /// <summary>
        /// Match the cases of the discriminated union with the corresponding functions.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="f1">The function to match with the first union case.</param>
        /// <param name="f2">The function to match with the second union case.</param>
        /// <param name="f3">The function to match with the third union case.</param>
        /// <returns>The result value.</returns>
        public abstract T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3);

        /// <summary>
        /// Match the cases of the discriminated union with the corresponding actions.
        /// </summary>
        /// <param name="f1">The action to match with the first union case.</param>
        /// <param name="f2">The action to match with the second union case.</param>
        /// <param name="f3">The action to match with the third union case.</param>
        public abstract void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3);

        private Union3() { }

        internal sealed class UnionCase1 : Union3<T1, T2, T3>
        {
            internal readonly T1 Item;

            internal UnionCase1(T1 item) 
                => Item = item;

            /// <inheritdoc />
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) 
                => f1 != null 
                    ? f1(Item) 
                    : default;

            /// <inheritdoc />
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) 
                => f1?.Invoke(Item);
        }

        internal sealed class UnionCase2 : Union3<T1, T2, T3>
        {
            internal readonly T2 Item;

            internal UnionCase2(T2 item) 
                => Item = item;

            /// <inheritdoc />
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) 
                => f2 != null 
                    ? f2(Item) 
                    : default;

            /// <inheritdoc />
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) 
                => f2?.Invoke(Item);
        }

        internal sealed class UnionCase3 : Union3<T1, T2, T3>
        {
            internal readonly T3 Item;

            internal UnionCase3(T3 item) 
                => Item = item;

            /// <inheritdoc />
            public override T Match<T>(Func<T1, T> f1, Func<T2, T> f2, Func<T3, T> f3) 
                => f3 != null 
                    ? f3(Item) 
                    : default;

            /// <inheritdoc />
            public override void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3) 
                => f3?.Invoke(Item);
        }

        /// <summary>
        /// Construct a value of the discriminated union using the first union case.
        /// </summary>
        /// <param name="value">The value of the first union case.</param>
        /// <returns>The discriminated union value.</returns>
        public static Union3<T1, T2, T3> Case1(T1 value) 
            => new UnionCase1(value);

        /// <summary>
        /// Construct a value of the discriminated union using the second union case.
        /// </summary>
        /// <param name="value">The value of the second union case.</param>
        /// <returns>The discriminated union value.</returns>
        public static Union3<T1, T2, T3> Case2(T2 value) 
            => new UnionCase2(value);

        /// <summary>
        /// Construct a value of the discriminated union using the third union case.
        /// </summary>
        /// <param name="value">The value of the third union case.</param>
        /// <returns>The discriminated union value.</returns>
        public static Union3<T1, T2, T3> Case3(T3 value) 
            => new UnionCase3(value);

        #region Equality

        public override bool Equals(object obj)
            => obj is Union3<T1, T2, T3> other
               && Equals(other);

        protected bool Equals(Union3<T1, T2, T3> other)
            => other != null
               && other.Match(
                   otherCase1 => Match(
                       thisCase1 => otherCase1.Equals(thisCase1),
                       thisCase2 => false,
                       thisCase3 => false),
                   otherCase2 => Match(
                       thisCase1 => false,
                       thisCase2 => otherCase2.Equals(thisCase2),
                       thisCase3 => false),
                   otherCase3 => Match(
                       thisCase1 => false,
                       thisCase2 => false,
                       thisCase3 => otherCase3.Equals(thisCase3)));

        public override int GetHashCode()
            => Match(
                case1 => case1.GetHashCode(),
                case2 => case2.GetHashCode(),
                case3 => case3.GetHashCode());

        #endregion
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