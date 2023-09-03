using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Output
{
    /// <summary>
    /// LoginMessageModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginMessageModelResponse
    {
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
        public static async Task<IResponse<LoginMessageModelResponse>> FromAsync(
            Task<IResponse<LoginMessageModelOutput>> task
        )
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static LoginMessageModelResponse FromGetPerfil(LoginMessageModelOutput output)
        {
            return FromProfileModelOutput(output);
        }

        private static LoginMessageModelResponse FromProfileModelOutput(LoginMessageModelOutput output)
        {
            return new LoginMessageModelResponse
            {
                Mensaje = output.Mensaje
            };
        }
    }
}
