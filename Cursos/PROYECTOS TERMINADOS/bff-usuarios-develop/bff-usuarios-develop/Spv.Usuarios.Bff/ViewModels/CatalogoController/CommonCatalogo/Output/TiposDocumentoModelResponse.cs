using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output
{
    /// <summary>
    /// TiposDocumentoModelResponse
    /// </summary>
    public class TiposDocumentoModelResponse
    {
        /// <summary>
        /// codigo
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public int Codigo { get; set; }

        /// <summary>
        /// descripcion
        /// </summary>
        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Constructor ValidacionModelResponse
        /// </summary>
        protected TiposDocumentoModelResponse()
        {
            /* empty constructor */
        }

        private static object FromGetTiposDocumento(List<TipoDocumentoModelOutput> output)
        {
            return FromTiposDocumentoModelOutput(output);
        }

        /// <summary>
        /// FromTiposDocumentoModelOutput
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static object FromTiposDocumentoModelOutput(List<TipoDocumentoModelOutput> output)
        {
            var response = new List<TiposDocumentoModelResponse>();
            output.ForEach(o => response.Add(
                new TiposDocumentoModelResponse
                {
                    Codigo = o.codigo, 
                    Descripcion = o.descripcion
                }));
            return response;
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<object>> FromAsync(Task<IResponse<List<TipoDocumentoModelOutput>>> task)
        {
            var response = await task;
            return response.Map(FromGetTiposDocumento);
        }
    }
}
