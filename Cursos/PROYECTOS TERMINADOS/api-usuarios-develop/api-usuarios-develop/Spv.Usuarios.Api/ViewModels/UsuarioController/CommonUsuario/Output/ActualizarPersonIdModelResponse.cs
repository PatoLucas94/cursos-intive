using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ActualizarPersonIdModelResponse
    /// </summary>
    public class ActualizarPersonIdModelResponse
    {
        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public string PersonId { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ActualizarPersonIdModelResponse>> FromAsync(
            Task<IResponse<ActualizarPersonIdModelOutput>> task
        )
        {
            var response = await task;
            return response.Map(FromValidarExistencia);
        }

        private static ActualizarPersonIdModelResponse FromValidarExistencia(ActualizarPersonIdModelOutput output)
        {
            return new ActualizarPersonIdModelResponse
            {
                PersonId = output.PersonId
            };
        }
    }
}
