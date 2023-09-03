using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Output
{
    /// <summary>
    /// BiometriaAutenticacionModelResponse
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BiometriaAutenticacionModelResponse
    {
        /// <summary>
        /// BiometriaAutenticacionModelResponse
        /// </summary>
        protected BiometriaAutenticacionModelResponse()
        {
            /* Empty constructor */
        }

        [JsonProperty("identificacion_digital")]
        public IdentificacionDigitalResponse IdentificacionDigital { get; set; }

        [JsonProperty("status_liveness_pasivo")]
        public string StatusLivenessPasivo { get; set; }

        [JsonProperty("status_obtenido_mejor_imagen_facial")]
        public string StatusObtenidoMejorImagenFacial { get; set; }

        [JsonProperty("status_obtenido_plantilla_facial")]
        public string StatusObtenidoPlantillaFacial { get; set; }

        [JsonProperty("status_obtenido_plantilla_facial_extendida")]
        public string StatusObtenidoPlantillaFacialExtendida { get; set; }

        [JsonProperty("validacion_manual")]
        public bool? ValidacionManual { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<BiometriaAutenticacionModelResponse>> FromAsync(
            Task<IResponse<BiometriaAutenticacionModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<BiometriaAutenticacionModelResponse>);
        }
    }
}
