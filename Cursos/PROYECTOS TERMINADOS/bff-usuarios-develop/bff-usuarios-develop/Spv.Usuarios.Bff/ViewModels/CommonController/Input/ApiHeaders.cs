using System;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.CommonController.Input
{
    /// <summary>
    /// Api Headers
    /// </summary>
    public class ApiHeaders
    {
        private string _xRequestId;

        /// <summary>
        /// Identificador único de petición.
        /// </summary>
        [FromHeader(Name = HeaderNames.RequestIdHeaderName)]
        public string XRequestId
        {
            get
            {
                if (IsSet())
                {
                    return _xRequestId;
                }

                _xRequestId = Guid.NewGuid().ToString();
                return _xRequestId;
            }
            set
            {
                if (IsValid(value) && !IsSet())
                {
                    _xRequestId = value;
                }
            }
        }

        private static bool IsValid(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        private bool IsSet()
        {
            return IsValid(_xRequestId);
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual IRequestBody<T> ToRequestBody<T>(T body)
        {
            return new RequestBody<T>(XRequestId, body);
        }
    }
}
