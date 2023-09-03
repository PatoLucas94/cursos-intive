using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses.Abstract;

namespace Spv.Usuarios.Bff.Common.ExternalResponses
{
    /// <summary>
    /// ApiUsuariosErrorResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiUsuariosErrorResponse : ApiErrorResponse<ApiUsuarioError>
    {
        /// <summary>
        /// ApiUsuariosErrorResponse
        /// </summary>
        public ApiUsuariosErrorResponse() : base()
        {
        }

        /// <summary>
        /// ApiUsuariosErrorResponse
        /// </summary>
        public ApiUsuariosErrorResponse(
            string state,
            int code,
            string type,
            string detail,
            List<ApiUsuarioError> errors
        ) : base(state, code, type, detail, errors)
        {
        }
    }
}
