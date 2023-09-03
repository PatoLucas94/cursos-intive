using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.Domain.Utils
{
    [ExcludeFromCodeCoverage]
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
        public Task<Result<TR>> MapAsync<TR>(Func<T, Task<TR>> mapper) => MapAsyncCore(Arg.NonNull(mapper, nameof(mapper)));

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
            v => new InvalidOperationException($"El valor '{v}' no cumple con el predicado '{filter.Method.Name}' especificado."));

        [DebuggerStepThrough]
        public Result<T> Where(Func<T, bool> filter, Func<T, Exception> exceptionBuilder) =>
            WhereCore(Arg.NonNull(filter, nameof(filter)), Arg.NonNull(exceptionBuilder, nameof(exceptionBuilder)));

        [DebuggerStepThrough]
        public IEnumerable<T> AsEnumerable(Action<Exception> errorConsumer) => AsEnumerableCore(Arg.NonNull(errorConsumer, nameof(errorConsumer)));

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
}
