using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Bff.Common.ExternalResponses.Abstract;

namespace Spv.Usuarios.Bff.Common.Errors
{
    /// <summary>
    /// ApiUsuarioError
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiUsuarioError : ApiGenericError
    {
        /// <summary>
        /// ApiUsuarioError
        /// </summary>
        public ApiUsuarioError() : base()
        {
        }

        /// <summary>
        /// ApiUsuarioError
        /// </summary>
        public ApiUsuarioError(string code, string title, string source, string detail, string spvTrackId)
            : base(code, title, source, detail, spvTrackId)
        {
        }
    }
}
