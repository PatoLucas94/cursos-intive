using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses.Abstract;

namespace Spv.Usuarios.Bff.Common.ExternalResponses
{
    /// <summary>
    /// ApiBiometriaErrorResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiBiometriaErrorResponse : ApiErrorResponse<ApiBiometriaError>
    {
        /// <summary>
        /// ApiBiometriaErrorResponse
        /// </summary>
        public ApiBiometriaErrorResponse() : base()
        {
        }

        /// <summary>
        /// ApiUsuariosErrorResponse
        /// </summary>
        public ApiBiometriaErrorResponse(
            string state,
            int code,
            string type,
            string detail,
            List<ApiBiometriaError> errors
        ) : base(state, code, type, detail, errors)
        {
        }
    }
}
