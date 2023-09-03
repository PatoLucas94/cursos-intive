using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Common.Constants;

namespace Spv.Usuarios.Api.Exceptions
{
    /// <summary>
    /// InvalidResponseBuilder
    /// </summary>
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
