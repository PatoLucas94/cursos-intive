using System.Collections.Generic;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;

namespace Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Output
{
    /// <summary>
    /// ConceptoModelResponse
    /// </summary>
    public class ConceptoModelResponse
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
        public static object FromConceptoModelOutput(List<ConceptoModelOutput> output)
        {
            var response = new List<ConceptoModelResponse>();
            output.ForEach(o => response.Add(
                new ConceptoModelResponse
                {
                    Codigo = o.codigo,
                    Descripcion = o.descripcion
                }));
            return response;
        }
    }
}
