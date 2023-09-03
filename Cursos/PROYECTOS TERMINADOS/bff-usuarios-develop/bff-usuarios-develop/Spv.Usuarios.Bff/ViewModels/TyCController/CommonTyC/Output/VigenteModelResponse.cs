using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output
{
    /// <summary>
    /// VigenteModelResponse
    /// </summary>
    public class VigenteModelResponse
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
        /// Canales
        /// </summary>
        [JsonProperty(PropertyName = "canales")]
        public List<CanalModelResponse> Canales { get; set; }

        /// <summary>
        /// Conceptos
        /// </summary>
        [JsonProperty(PropertyName = "conceptos")]
        public List<ConceptoModelResponse> Conceptos { get; set; }

        private static VigenteModelResponse FromPostTyCVigente(VigenteModelOutput output)
        {
            return FromTyCVigenteModelOutput(output);
        }

        private static VigenteModelResponse FromTyCVigenteModelOutput(VigenteModelOutput output)
        {
            return new VigenteModelResponse
            {
                Id = output.id,
                VigenciaDesde = output.vigencia_desde,
                Contenido = output.contenido,
                Canales = (List<CanalModelResponse>)CanalModelResponse.FromCanalModelOutput(output.canales),
                Conceptos = (List<ConceptoModelResponse>)ConceptoModelResponse.FromConceptoModelOutput(output.conceptos)
            };
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<VigenteModelResponse>> FromAsync(Task<IResponse<VigenteModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromPostTyCVigente);
        }
    }
}
