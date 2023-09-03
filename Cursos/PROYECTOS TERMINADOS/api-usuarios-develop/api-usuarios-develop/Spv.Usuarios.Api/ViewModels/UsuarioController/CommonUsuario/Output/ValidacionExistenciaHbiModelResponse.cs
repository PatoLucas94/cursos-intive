using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ValidacionExistenciaHbiModelResponse
    /// </summary>
    public class ValidacionExistenciaHbiModelResponse
    {
        /// <summary>
        /// Constructor ValidacionExistenciaHbiModelResponse
        /// </summary>
        protected ValidacionExistenciaHbiModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Existe usuario
        /// </summary>
        [JsonProperty(PropertyName = "existe_usuario")]
        public bool ExisteUsuario { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaHbiModelResponse>> FromAsync(Task<IResponse<ValidacionExistenciaHbiModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromValidarExistencia);
        }

        private static ValidacionExistenciaHbiModelResponse FromValidarExistencia(ValidacionExistenciaHbiModelOutput output)
        {
            return FromValidacionExistenciaModelOutput(output);
        }

        private static ValidacionExistenciaHbiModelResponse FromValidacionExistenciaModelOutput(ValidacionExistenciaHbiModelOutput output)
        {
            return new ValidacionExistenciaHbiModelResponse
            {
                ExisteUsuario = output.ExisteUsuario
            };
        }
    }
}
