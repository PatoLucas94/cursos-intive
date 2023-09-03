using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spv.Usuarios.Bff.Domain.Exceptions;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Exceptions;

namespace Spv.Usuarios.Bff.Filters
{
    /// <summary>
    /// ExceptionsAttribute
    /// </summary>
    public class ExceptionsAttribute : Attribute, IExceptionFilter
    {
        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnException(ExceptionContext context)
        {
            var statusCode = SetErrorCode(context);

            var errorDetail = new ErrorDetailModel
            {
                State = ((HttpStatusCode)statusCode).ToString(),
                Code = statusCode
            };

            SetExceptionType(context, errorDetail);

            errorDetail.Detail = context.Exception.Message;
            errorDetail.Errors.Add(
                new ApiError(
                    statusCode.ToString(),
                    context.Exception.Message,
                    context.Exception.Source,
                    context.Exception.InnerException?.Message ?? string.Empty,
                    context.HttpContext.TraceIdentifier));

            context.Result = new ObjectResult(errorDetail);
            context.HttpContext.Response.StatusCode = statusCode;
        }

        private static void SetExceptionType(ExceptionContext context, ErrorDetailModel errorDetail)
        {
            var exceptionType = context.Exception.GetType();
            errorDetail.Type = exceptionType.Name switch
            {
                // Checking for Business Exceptions
                nameof(BusinessException) => ErrorTypeConstants.Business,
                _ => ErrorTypeConstants.Technical
            };
        }

        private static int SetErrorCode(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();

            switch (exceptionType.Name)
            {
                case nameof(BusinessException):
                    {
                        var data = (BusinessException)context.Exception;

                        return data.Code > 0 ? data.Code : StatusCodes.Status500InternalServerError;
                    }
                case nameof(Exception):
                    {
                        return StatusCodes.Status500InternalServerError;
                    }
                default:
                    return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
