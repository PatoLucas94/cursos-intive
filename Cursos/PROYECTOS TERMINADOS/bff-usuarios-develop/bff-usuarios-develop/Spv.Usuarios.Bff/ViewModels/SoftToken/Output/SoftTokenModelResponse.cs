using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.SoftToken.Output
{
    /// <summary>
    /// SoftTokenModelResponse
    /// </summary>
    public class SoftTokenModelResponse
    {
        /// <summary>
        /// Detalle
        /// </summary>
        [JsonProperty(PropertyName = "Detalle")]
        public string Detalle { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        [JsonProperty(PropertyName = "Estado")]
        public string Estado { get; set; }

        /// <summary>
        /// Bloqueado
        /// </summary>
        [JsonProperty(PropertyName = "Bloqueado")]
        public bool Bloqueado { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [JsonProperty(PropertyName = "Identificador")]
        public string Identificador { get; set; }

        private static SoftTokenModelResponse FromPostSoftTokenHabilitado(SoftTokenModelOutput output)
        {
            return FromSoftTokenHabilitadoModelOutput(output);
        }

        private static SoftTokenModelResponse FromSoftTokenHabilitadoModelOutput(SoftTokenModelOutput output)
        {
            return new SoftTokenModelResponse
            {
                Detalle = output.Detalle,
                Estado = output.Estado,
                Bloqueado = output.Bloqueado,
                Identificador = output.Identificador
            };
        }
        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<SoftTokenModelResponse>> FromAsync(Task<IResponse<SoftTokenModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromPostSoftTokenHabilitado);
        }
    }
}
