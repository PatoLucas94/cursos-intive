using System.Collections.Generic;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;

namespace Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output
{
    /// <summary>
    /// CanalModelResponse
    /// </summary>
    public class CanalModelResponse
    {
        /// <summary>
        /// Código
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// FromConceptoModelOutput
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static object FromCanalModelOutput(List<CanalModelOutput> output)
        {
            var response = new List<CanalModelResponse>();
            output.ForEach(o => response.Add(
                new CanalModelResponse
                {
                    Codigo = o.codigo,
                    Descripcion = o.descripcion
                }));
            return response;
        }
    }
}
