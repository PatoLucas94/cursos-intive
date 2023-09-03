using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// ValidacionExistenciaHbiModelRequest
    /// </summary>
    public class ValidacionExistenciaHbiModelRequest
    {
        /// <summary>
        /// NombreUsuario
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NombreUsuario)]
        public string NombreUsuario { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ValidacionExistenciaHbiModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ValidacionExistenciaHbiModelInput
                {
                    UserName = NombreUsuario
                });
        }
    }
}
