using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ValidacionExistenciaHbiModelResponse
    /// </summary>
    public class ValidacionExistenciaHbiModelResponse
    {
        /// <summary>
        /// Existe usuario
        /// </summary>
        [JsonProperty(PropertyName = "existe_usuario")]
        public bool ExisteUsuario { get; set; }

        /// <summary>
        /// Constructor ValidacionExistenciaHbiModelResponse
        /// </summary>
        protected ValidacionExistenciaHbiModelResponse()
        {
            /* Empty constructor */
        }

        private static ValidacionExistenciaHbiModelResponse FromGetValidacionExistencia(ValidacionExistenciaHbiModelOutput output)
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

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaHbiModelResponse>> FromAsync(Task<IResponse<ValidacionExistenciaHbiModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetValidacionExistencia);
        }

    }
}
