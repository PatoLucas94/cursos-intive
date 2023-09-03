using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.ConfiguracionesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.ConfiguracionesController.Output
{
    /// <summary> 
    /// TerminosYCondicionesHabilitadoModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TerminosYCondicionesHabilitadoModelResponse
    {
        /// <summary>
        /// Habilitado
        /// </summary>
        [JsonProperty(PropertyName = "habilitado")]
        public bool Habilitado { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<TerminosYCondicionesHabilitadoModelResponse>> FromAsync(
            Task<IResponse<TerminosYCondicionesHabilitadoModelOutput>> task
        )
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static TerminosYCondicionesHabilitadoModelResponse FromGetPerfil(
            TerminosYCondicionesHabilitadoModelOutput output
        )
        {
            return FromProfileModelOutput(output);
        }

        private static TerminosYCondicionesHabilitadoModelResponse FromProfileModelOutput(
            TerminosYCondicionesHabilitadoModelOutput output
        )
        {
            return new TerminosYCondicionesHabilitadoModelResponse
            {
                Habilitado = output.Habilitado
            };
        }
    }
}
