using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Spv.Usuarios.Bff.Domain.Utils
{
    [ExcludeFromCodeCoverage]
    public sealed class OkResult<TI> : Result<TI>
    {
        private readonly TI _value;

        [DebuggerStepThrough]
        public OkResult(TI value) => _value = value;

        [DebuggerStepThrough]
        protected override Result<TO> MapCore<TO>(Func<TI, TO> mapper) => Result.Ok(mapper(_value));

        [DebuggerStepThrough]
        protected override async Task<Result<TO>> MapAsyncCore<TO>(Func<TI, Task<TO>> mapper) => Result.Ok(await mapper(_value).ConfigureAwait(false));

        [DebuggerStepThrough]
        protected override Result<TO> BindCore<TO>(Func<TI, Result<TO>> binder) => binder(_value);

        [DebuggerStepThrough]
        protected override TI OrElseCore(Func<Exception, TI> mapper) => _value;

        [DebuggerStepThrough]
        protected override void DoCore(Action<TI> onOk, Action<Exception> onError) => onOk(_value);

        [DebuggerStepThrough]
        protected override TR MatchCore<TR>(Func<TI, TR> onOk, Func<Exception, TR> onError) => onOk(_value);

        [DebuggerStepThrough]
        protected override Result<TI> WhereCore(Func<TI, bool> filter, Func<TI, Exception> exceptionBuilder) => filter(_value) ? this : Result.Error<TI>(exceptionBuilder(_value));

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
}