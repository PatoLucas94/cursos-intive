using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// PerfilModelResponse
    /// </summary>
    [JsonObject(Title = "usuario")]
    public class PerfilModelResponse
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }



        private static PerfilModelResponse FromGetPerfil(PerfilModelOutput output)
        {
            return FromPerfilModelOutput(output);
        }

        private static PerfilModelResponse FromPerfilModelOutput(PerfilModelOutput output)
        {
            return new PerfilModelResponse
            {
                Id = output.PersonId,
            };
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilModelResponse>> FromAsync(Task<IResponse<PerfilModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

    }
}
