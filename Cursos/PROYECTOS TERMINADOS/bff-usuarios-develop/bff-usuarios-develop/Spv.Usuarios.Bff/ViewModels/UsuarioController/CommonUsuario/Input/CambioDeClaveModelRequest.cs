using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Cambio de clave model
    /// </summary>
    [JsonObject(Title = "CambioDeClaveModelRequest")]
    public class CambioDeClaveModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Nueva Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NuevaClave)]
        public string NuevaClave { get; set; }

        /// <summary>
        /// Clave de Canales
        /// </summary>
        [FromHeader(Name = ParameterNames.ClaveCanales)]
        [JsonProperty(PropertyName = ParameterNames.ClaveCanales)]
        public string ClaveCanales { get; set; }

        /// <summary>
        /// IsClaveCanales
        /// </summary>
        [FromHeader(Name = ParameterNames.IsClaveCanales)]
        [JsonProperty(PropertyName = ParameterNames.IsClaveCanales)]
        public bool IsClaveCanales { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con nueva_clave y clave_canales ofuscados
        /// </summary>
        public override string ToString()
        {
            return "{ IdPersona: '" + IdPersona + "', nueva_clave: '************', clave_canales: '************' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<CambioDeClaveModelInput> ToRequestBody(ApiHeadersDeviceId headers)
        {
            return headers?.ToRequestBody(
                new CambioDeClaveModelInput()
                {
                    PersonId = IdPersona,
                    NewPassword = NuevaClave,
                    ChannelKey = ClaveCanales,
                    IsChannelKey = IsClaveCanales,
                    DeviceId = headers.DeviceId
                });
        }
    }
}
