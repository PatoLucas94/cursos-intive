using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Spv.Usuarios.Domain.Utils
{
    public static class Result
    {
        #region factory methods

        [DebuggerStepThrough]
        public static Result<TR> Of<TR>(Func<TR> function)
        {
            Arg.NonNull(function, nameof(function));
            try
            {
                return Ok(function());
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case OutOfMemoryException _: throw;
                    case StackOverflowException _: throw;
                    default: return Error<TR>(e);
                }
            }
        }

        [DebuggerStepThrough]
        public static Result<TI> Ok<TI>(TI value)
        {
            return new OkResult<TI>(value);
        }

        [DebuggerStepThrough]
        public static Result<TI> Error<TI>(Exception exception)
        {
            return new ErrorResult<TI>(Arg.NonNull(exception, nameof(exception)));
        }

        #endregion
    }

    [ExcludeFromCodeCoverage]
    public abstract class Result<T>
    {
        private static Action<TR> NoOpAction<TR>() => t => { };

        [DebuggerStepThrough]
        public Result<TR> Map<TR>(Func<T, TR> mapper) => MapCore(Arg.NonNull(mapper, nameof(mapper)));

        [DebuggerStepThrough]
        public Result<TR> Select<TR>(Func<T, TR> mapper)
        {
            return Map(mapper);
        }

        [DebuggerStepThrough]
        public Task<Result<TR>> MapAsync<TR>(Func<T, Task<TR>> mapper) =>
            MapAsyncCore(Arg.NonNull(mapper, nameof(mapper)));

        [DebuggerStepThrough]
        public Result<TR> Bind<TR>(Func<T, Result<TR>> binder) => BindCore(Arg.NonNull(binder, nameof(binder)));

        [DebuggerStepThrough]
        public Result<TR> SelectMany<TS, TR>(Func<T, Result<TS>> binder, Func<T, TS, TR> selector)
        {
            Arg.NonNull(binder, nameof(binder));
            Arg.NonNull(selector, nameof(selector));
            return BindCore(t => binder(t).Map(s => selector(t, s)));
        }

        [DebuggerStepThrough]
        public Result<T> Where(Func<T, bool> filter) => WhereCore(Arg.NonNull(filter, nameof(filter)),
            v => new InvalidOperationException(
                $"El valor '{v}' no cumple con el predicado '{filter.Method.Name}' especificado."));

        [DebuggerStepThrough]
        public Result<T> Where(Func<T, bool> filter, Func<T, Exception> exceptionBuilder) =>
            WhereCore(Arg.NonNull(filter, nameof(filter)), Arg.NonNull(exceptionBuilder, nameof(exceptionBuilder)));

        [DebuggerStepThrough]
        public IEnumerable<T> AsEnumerable(Action<Exception> errorConsumer) =>
            AsEnumerableCore(Arg.NonNull(errorConsumer, nameof(errorConsumer)));

        [DebuggerStepThrough]
        protected abstract IEnumerable<T> AsEnumerableCore(Action<Exception> errorConsumer);

        [DebuggerStepThrough]
        public T OrElseDefault() => OrElseCore(e => default);

        [DebuggerStepThrough]
        public T OrElse(Func<Exception, T> mapper) => OrElseCore(Arg.NonNull(mapper, nameof(mapper)));

        [DebuggerStepThrough]
        public TR Match<TR>(Func<T, TR> onOk, Func<Exception, TR> onError)
        {
            return MatchCore(Arg.NonNull(onOk, nameof(onOk)), Arg.NonNull(onError, nameof(onError)));
        }

        [DebuggerStepThrough]
        public Result<T> OnOk(Action<T> action)
        {
            DoCore(Arg.NonNull(action, nameof(action)), NoOpAction<Exception>());
            return this;
        }

        [DebuggerStepThrough]
        public Result<T> OnError(Action<Exception> action)
        {
            DoCore(NoOpAction<T>(), Arg.NonNull(action, nameof(action)));
            return this;
        }

        protected abstract Result<TR> BindCore<TR>(Func<T, Result<TR>> binder);

        protected abstract Result<TR> MapCore<TR>(Func<T, TR> mapper);

        protected abstract Task<Result<TR>> MapAsyncCore<TR>(Func<T, Task<TR>> mapper);

        protected abstract T OrElseCore(Func<Exception, T> mapper);

        protected abstract void DoCore(Action<T> onOk, Action<Exception> onError);

        protected abstract TR MatchCore<TR>(Func<T, TR> onOk, Func<Exception, TR> onError);

        protected abstract Result<T> WhereCore(Func<T, bool> filter, Func<T, Exception> exceptionBuilder);
    }

    [ExcludeFromCodeCoverage]
    public sealed class OkResult<TI> : Result<TI>
    {
        private readonly TI _value;

        [DebuggerStepThrough]
        public OkResult(TI value) => _value = value;

        [DebuggerStepThrough]
        protected override Result<TO> MapCore<TO>(Func<TI, TO> mapper) => Result.Ok(mapper(_value));

        [DebuggerStepThrough]
        protected override async Task<Result<TO>> MapAsyncCore<TO>(Func<TI, Task<TO>> mapper) =>
            Result.Ok(await mapper(_value).ConfigureAwait(false));

        [DebuggerStepThrough]
        protected override Result<TO> BindCore<TO>(Func<TI, Result<TO>> binder) => binder(_value);

        [DebuggerStepThrough]
        protected override TI OrElseCore(Func<Exception, TI> mapper) => _value;

        [DebuggerStepThrough]
        protected override void DoCore(Action<TI> onOk, Action<Exception> onError) => onOk(_value);

        [DebuggerStepThrough]
        protected override TR MatchCore<TR>(Func<TI, TR> onOk, Func<Exception, TR> onError) => onOk(_value);

        [DebuggerStepThrough]
        protected override Result<TI> WhereCore(Func<TI, bool> filter, Func<TI, Exception> exceptionBuilder) =>
            filter(_value) ? this : Result.Error<TI>(exceptionBuilder(_value));

        [DebuggerStepThrough]
        protected override IEnumerable<TI> AsEnumerableCore(Action<Exception> errorConsumer)
        {
            yield return _value;
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj) => obj is OkResult<TI> other && Equals(_value, other._value);

        [DebuggerStepThrough]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;

        [DebuggerStepThrough]
        public override string ToString() => $"{nameof(Result)}.Ok<{nameof(TI)}>({_value})";
    }

    [ExcludeFromCodeCoverage]
    public sealed class ErrorResult<TI> : Result<TI>
    {
        private readonly Exception _exception;

        [DebuggerStepThrough]
        public ErrorResult(Exception exception) => _exception = exception;

        [DebuggerStepThrough]
        protected override Result<TO> MapCore<TO>(Func<TI, TO> mapper) => Result.Error<TO>(_exception);

        [DebuggerStepThrough]
        protected override Task<Result<TO>> MapAsyncCore<TO>(Func<TI, Task<TO>> mapper) =>
            Task.FromResult(Result.Error<TO>(_exception));

        [DebuggerStepThrough]
        protected override Result<TO> BindCore<TO>(Func<TI, Result<TO>> binder) => Result.Error<TO>(_exception);

        [DebuggerStepThrough]
        protected override TI OrElseCore(Func<Exception, TI> mapper) => mapper(_exception);

        [DebuggerStepThrough]
        protected override void DoCore(Action<TI> onOk, Action<Exception> onError) => onError(_exception);

        [DebuggerStepThrough]
        protected override TR MatchCore<TR>(Func<TI, TR> onOk, Func<Exception, TR> onError) => onError(_exception);

        [DebuggerStepThrough]
        protected override Result<TI> WhereCore(Func<TI, bool> filter, Func<TI, Exception> exceptionBuilder) => this;

        [DebuggerStepThrough]
        protected override IEnumerable<TI> AsEnumerableCore(Action<Exception> errorConsumer)
        {
            errorConsumer(_exception);
            return Enumerable.Empty<TI>();
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj) => obj is ErrorResult<TI> other && Equals(_exception, other._exception);

        [DebuggerStepThrough]
        public override int GetHashCode() => _exception.GetHashCode();

        [DebuggerStepThrough]
        public override string ToString() => $"{nameof(Result)}.Error<{nameof(TI)}>: {_exception.Message}";
    }
}
