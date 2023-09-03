using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace Spv.Usuarios.Bff.Domain.Exceptions
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

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
