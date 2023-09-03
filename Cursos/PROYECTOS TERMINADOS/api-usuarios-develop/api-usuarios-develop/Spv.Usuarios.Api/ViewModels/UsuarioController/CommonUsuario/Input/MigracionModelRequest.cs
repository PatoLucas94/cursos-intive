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
    /// Migración model
    /// </summary>
    public class MigracionModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long IdPersona { get; set; }

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
        /// Retorna la representación en json del objeto clave y usuario ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.IdPersona}:'{IdPersona}', {ParameterNames.Usuario}"+
                   $":'************', {ParameterNames.NuevaClave }:'************' }}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<MigracionModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new MigracionModelInput
                {
                    IdPersona = IdPersona,
                    UserName = Usuario,
                    Password = Clave
                }, allowedChannels);
        }
    }
}
