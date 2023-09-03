using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.ConfiguracionService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ConfiguracionController.Output
{
    /// <summary>
    /// LoginHabilitadoModelResponse
    /// </summary>
    public class LoginHabilitadoModelResponse
    {
        /// <summary>
        /// Habilitado
        /// </summary>
        [JsonProperty(PropertyName = "habilitado")]
        public string Habilitado { get; set; }

        /// <summary>
        /// Mensaje
        /// </summary>
        [JsonProperty(PropertyName = "mensaje")]
        public string Mensaje { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<LoginHabilitadoModelResponse>> FromAsync(Task<IResponse<LoginHabilitadoModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetLoginHabilitado);
        }

        private static LoginHabilitadoModelResponse FromGetLoginHabilitado(LoginHabilitadoModelOutput output)
        {
            return FromLoginHabilitadoModelOutput(output);
        }

        private static LoginHabilitadoModelResponse FromLoginHabilitadoModelOutput(LoginHabilitadoModelOutput output)
        {
            return new LoginHabilitadoModelResponse
            {
                Habilitado = output.Habilitado,
                Mensaje = output.Mensaje
            };
        }
    }
}
