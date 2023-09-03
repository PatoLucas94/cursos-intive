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
    /// Migración model
    /// </summary>
    public class MigracionModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [FromHeader(Name = ParameterNames.Usuario), DomainValidation(typeof(CredUsuario))]
        [JsonProperty(PropertyName = ParameterNames.Usuario)]
        public string Usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [FromHeader(Name = ParameterNames.Clave), DomainValidation(typeof(CredClave))]
        [JsonProperty(PropertyName = ParameterNames.Clave)]
        public string Clave { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con Clave ofuscada
        /// </summary>
        public override string ToString()
        {
            return "{ IdPersona: '" + IdPersona + "', Usuario:'************' , Clave: '************' }";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<MigracionModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new MigracionModelInput
                {
                    PersonId = IdPersona,
                    UserName = Usuario,
                    Password = Clave
                });
        }
    }
}
