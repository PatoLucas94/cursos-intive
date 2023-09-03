using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// PerfilModelRequest
    /// </summary>
    [JsonObject(Title = "ValidacionModelRequest")]
    [ExcludeFromCodeCoverage]
    public class PerfilModelRequest
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [FromQuery(Name = ParameterNames.NombreUsuario)]
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto
        /// </summary>
        public override string ToString()
        {
            return "{ usuario: '" + NombreUsuario + "'}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<PerfilModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new PerfilModelInput
                {
                    UserName = NombreUsuario
                });
        }
    }
}
