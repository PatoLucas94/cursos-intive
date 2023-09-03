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
    /// Cambio de credenciales model v2
    /// </summary>
    [JsonObject(Title = "CambioDeCredencialesModelRequest")]
    public class CambioDeCredencialesModelRequestV2
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long IdPersona { get; set; }

        /// <summary>
        /// Nuevo Usuario
        /// </summary>
        [FromHeader(Name = ParameterNames.NuevoUsuario), DomainValidation(typeof(CredUsuario))]
        [JsonProperty(PropertyName = ParameterNames.NuevoUsuario)]
        public string NuevoUsuario { get; set; }

        /// <summary>
        /// Nueva Clave
        /// </summary>
        [FromHeader(Name = ParameterNames.NuevaClave), DomainValidation(typeof(CredClave))]
        [JsonProperty(PropertyName = ParameterNames.NuevaClave)]
        public string NuevaClave { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con la NuevaClave y el NuevoUsuario ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.IdPersona}:'{IdPersona }', {ParameterNames.NuevoUsuario}" +
                   $":'************', {ParameterNames.NuevaClave}: '************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<CambioDeCredencialesModelInputV2> ToRequestBody(ApiHeadersGateWay headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new CambioDeCredencialesModelInputV2
                {
                    PersonId = IdPersona,
                    NewUsername = NuevoUsuario,
                    NewPassword = NuevaClave
                }, allowedChannels);
        }
    }
}