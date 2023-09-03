using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Domain.Exceptions
{
    /// <summary>
    /// Main class for Business Exception builders
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BusinessException : Exception
    {
        public int Code { get; set; }
        public IDictionary<int, string[]> Errors { get; set; }

        public BusinessException(string message, int code)
            : base(message)
        {
            Code = code;
        }

        public BusinessException(EventId eventId) : base(eventId.Name)
        {
            Code = eventId.Id;
            Errors = new Dictionary<int, string[]> { { Code, new[] { eventId.Name } } };
        }

        protected BusinessException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
