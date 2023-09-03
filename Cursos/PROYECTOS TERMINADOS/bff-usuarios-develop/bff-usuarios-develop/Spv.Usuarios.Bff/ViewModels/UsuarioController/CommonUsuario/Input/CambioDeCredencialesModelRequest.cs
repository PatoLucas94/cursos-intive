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
    /// Cambio de credenciales model
    /// </summary>
    [JsonObject(Title = "CambioDeCredencialesModelRequest")]
    public class CambioDeCredencialesModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

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
            return "{ IdPersona: '" + IdPersona + "nuevo_usuario: '************', nueva_clave: '************', clave_canales: '************' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<CambioDeCredencialesModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new CambioDeCredencialesModelInput
                {
                    PersonId = IdPersona,
                    NewUsername = NuevoUsuario,
                    NewPassword = NuevaClave,
                    ChannelKey = ClaveCanales,
                    IsChannelKey = IsClaveCanales
                });
        }
    }
}
