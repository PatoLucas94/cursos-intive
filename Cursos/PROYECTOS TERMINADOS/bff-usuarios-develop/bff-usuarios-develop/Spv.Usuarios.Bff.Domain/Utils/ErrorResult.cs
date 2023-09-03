using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.Domain.Utils
{
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
