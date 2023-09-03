using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// CambioEstadoModelRequest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CambioEstadoModelRequest
    {
        /// <summary>
        /// Identificador de la Persona.
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long PersonId { get; set; }

        /// <summary>
        /// Identificador del estado a Ccambiar.
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdEstado)]
        public byte EstadoId { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<CambioEstadoModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
            => headers?.ToRequestBody(
                new CambioEstadoModelInput
                {
                    PersonId = PersonId,
                    EstadoId = (UserStatus)EstadoId
                },
                allowedChannels
            );
    }
}