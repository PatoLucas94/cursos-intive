using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Exceptions
{
    /// <summary>
    /// InvalidResponseBuilder
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvalidResponseBuilder : IInvalidResponseBuilder
    {
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult Build(ActionExecutingContext context)
        {
            var response = new ErrorDetailModel
            {
                State = HttpStatusCode.BadRequest.ToString(),
                Code = (int)HttpStatusCode.BadRequest,
                Type = ErrorTypeConstants.Business,
                Detail = MessageConstants.ErrorDeValidacion
            };

            foreach (var error in context.ModelState.SelectMany(item => item.Value.Errors))
            {
                response.Errors.Add(
                    new ApiError(
                        ErrorConstants.CodigoCampoRequerido,
                        MessageConstants.CampoInvalido,
                        context.ActionDescriptor.DisplayName,
                        error.ErrorMessage,
                        context.HttpContext.TraceIdentifier));
            }

            return new BadRequestObjectResult(response);
        }
    }
}
