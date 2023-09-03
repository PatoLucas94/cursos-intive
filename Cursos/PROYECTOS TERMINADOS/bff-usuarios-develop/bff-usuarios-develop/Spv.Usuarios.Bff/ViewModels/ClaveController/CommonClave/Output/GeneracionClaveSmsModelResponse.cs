using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.ClaveService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Output
{
    /// <summary>
    /// GeneracionClaveSmsModelResponse
    /// </summary>
    public class GeneracionClaveSmsModelResponse
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [JsonProperty(PropertyName = "identificador")]
        public string Identificador { get; set; }

        /// <summary>
        /// Constructor GeneracionClaveSmsModelResponse
        /// </summary>
        protected GeneracionClaveSmsModelResponse()
        {
            /* Empty constructor */
        }

        private static GeneracionClaveSmsModelResponse FromGetGeneracionClaveSms(GeneracionClaveSmsModelOutput output)
        {
            return FromGeneracionClaveSmsModelOutput(output);
        }

        private static GeneracionClaveSmsModelResponse FromGeneracionClaveSmsModelOutput(GeneracionClaveSmsModelOutput output)
        {
            return new GeneracionClaveSmsModelResponse
            {
                Identificador = output.Identificador
            };
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<GeneracionClaveSmsModelResponse>> FromAsync(Task<IResponse<GeneracionClaveSmsModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetGeneracionClaveSms);
        }
    }
}