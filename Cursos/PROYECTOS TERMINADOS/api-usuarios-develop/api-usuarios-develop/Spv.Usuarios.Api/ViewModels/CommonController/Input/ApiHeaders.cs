using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.CommonController.Input
{
    /// <summary>
    /// Api Headers
    /// </summary>
    public class ApiHeaders
    {
        /// <summary>
        /// Canal desde donde se generó la petición.
        /// </summary>
        [FromHeader(Name = HeaderNames.ChannelHeaderName),
         Required(ErrorMessage = MessageConstants.ChannelHeaderRequiredMessage), MinLength(1)]
        public string XCanal { get; set; }

        /// <summary>
        /// Usuario que generó la petición.
        /// </summary>
        [FromHeader(Name = HeaderNames.UserHeaderName),
         Required(ErrorMessage = MessageConstants.UserHeaderRequiredMessage), MinLength(1)]
        public string XUsuario { get; set; }

        /// <summary>
        /// Aplicación desde la que se generó la petición.
        /// </summary>
        [FromHeader(Name = HeaderNames.ApplicationHeaderName),
         Required(ErrorMessage = MessageConstants.ApplicationHeaderRequiredMessage), MinLength(1)]
        public string XAplicacion { get; set; }

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
        /// <param name="allowedChannels"></param>
        /// <returns></returns>
        public virtual IRequestBody<T> ToRequestBody<T>(T body, IAllowedChannels allowedChannels)
        {
            if (!AllowedChannel(XCanal, allowedChannels))
            {
                throw new BusinessException(
                    MessageConstants.ChannelHeaderInvalidMessage,
                    StatusCodes.Status401Unauthorized
                );
            }

            return new RequestBody<T>(XRequestId, XCanal, XUsuario, XAplicacion, body);
        }

        private static bool AllowedChannel(string channel, IAllowedChannels allowedChannels) =>
            allowedChannels.IsValidChannel(channel);
    }
}
