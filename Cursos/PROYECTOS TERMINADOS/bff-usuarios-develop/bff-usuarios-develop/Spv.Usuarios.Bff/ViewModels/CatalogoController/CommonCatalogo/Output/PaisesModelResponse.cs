using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output
{
    /// <summary>
    /// PaisesModelResponse
    /// </summary>
    public class PaisesModelResponse
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
        /// Constructor PaisesModelResponse
        /// </summary>
        protected PaisesModelResponse()
        {
            /* empty constructor */
        }

        private static object FromGetPaises(List<PaisModelOutput> output)
        {
            return FromPaisesModelOutput(output);
        }

        /// <summary>
        /// FromPaisesModelOutput
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static object FromPaisesModelOutput(List<PaisModelOutput> output)
        {
            var response = new List<PaisesModelResponse>();
            output.ForEach(o => response.Add(
                new PaisesModelResponse
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
        public static async Task<IResponse<object>> FromAsync(Task<IResponse<List<PaisModelOutput>>> task)
        {
            var response = await task;
            return response.Map(FromGetPaises);
        }
    }
}
