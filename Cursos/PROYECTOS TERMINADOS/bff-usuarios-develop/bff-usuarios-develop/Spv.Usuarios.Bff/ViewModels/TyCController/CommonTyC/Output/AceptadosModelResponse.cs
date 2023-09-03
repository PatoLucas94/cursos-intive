using System;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output
{
    /// <summary>
    /// AceptadosModelResponse
    /// </summary>
    public class AceptadosModelResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Vigencia Desde
        /// </summary>
        [JsonProperty(PropertyName = "vigencia_desde")]
        public DateTime VigenciaDesde { get; set; }

        /// <summary>
        /// Contenido
        /// </summary>
        [JsonProperty(PropertyName = "contenido")]
        public string Contenido { get; set; }

        /// <summary>
        /// Aceptados
        /// </summary>
        [JsonProperty(PropertyName = "aceptados")]
        public bool Aceptados { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<AceptadosModelResponse>> FromAsync(
            Task<IResponse<AceptadosModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<AceptadosModelResponse>);
        }
    }
}
