using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Common.Attributes;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Cambio de clave model v2
    /// </summary>
    [JsonObject(Title = "CambioDeClaveModelRequest")]
    public class CambioDeClaveModelRequestV2
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long IdPersona { get; set; }

        /// <summary>
        /// Nueva Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NuevaClave)]
        public string NuevaClave { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Clave)]
        public string Clave { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto clave ofuscados
        /// </summary>
        public override string ToString()
        {
            return
                $"{{ {ParameterNames.IdPersona}:'{IdPersona}', {ParameterNames.NuevaClave}:'************', {ParameterNames.Clave}:'************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<CambioDeClaveModelInputV2> ToRequestBody(
            ApiHeadersGateWay headers,
            IAllowedChannels allowedChannels
        ) => headers?.ToRequestBody(
            new CambioDeClaveModelInputV2
            {
                PersonId = IdPersona,
                NewPassword = NuevaClave,
                CurrentPasword = Clave,
            },
            allowedChannels
        );
    }
}
