using System.Diagnostics.CodeAnalysis;
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
    /// PerfilModelRequest
    /// </summary>
    [JsonObject(Title = "PerfilModelRequest")]
    [ExcludeFromCodeCoverage]
    public class PerfilModelRequestV2
    {
        /// <summary>
        /// Id Persona
        /// </summary>
        [FromQuery(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        public long IdPersona { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto
        /// </summary>
        public override string ToString()
        {
            return "{ IdPersona: '" + IdPersona + "'}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PerfilModelInputV2> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new PerfilModelInputV2
                {
                    IdPersona = IdPersona
                });
        }
    }
}
