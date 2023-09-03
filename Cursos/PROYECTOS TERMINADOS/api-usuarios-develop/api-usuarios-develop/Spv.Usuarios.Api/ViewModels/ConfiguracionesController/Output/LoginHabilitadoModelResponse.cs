using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Output
{
    /// <summary>
    /// LoginHabilitadoModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginHabilitadoModelResponse
    {
        /// <summary>
        /// Habilitado
        /// </summary>
        [JsonProperty(PropertyName = "habilitado")]
        public string Habilitado { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<LoginHabilitadoModelResponse>> FromAsync(
            Task<IResponse<LoginHabilitadoModelOutput>> task
        )
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static LoginHabilitadoModelResponse FromGetPerfil(LoginHabilitadoModelOutput output)
        {
            return FromProfileModelOutput(output);
        }

        private static LoginHabilitadoModelResponse FromProfileModelOutput(LoginHabilitadoModelOutput output)
        {
            return new LoginHabilitadoModelResponse
            {
                Habilitado = output.Habilitado
            };
        }
    }
}
