using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.BiometriaController.CommonBiometria.Input
{
    /// <summary>
    /// BiometriaAutenticacionModelRequest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BiometriaAutenticacionModelRequest
    {
        /// <summary>
        /// IdPersona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long IdPersona { get; set; }

        /// <summary>
        /// DatosBiometricos
        /// </summary>
        [JsonProperty(PropertyName = "datos_biometricos")]
        public DatosBiometricosModelRequest DatosBiometricos { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<BiometriaAutenticacionModelInput> ToRequestBody(ApiHeaders headers, IMapper mapper) =>
            headers?.ToRequestBody(mapper.Map<BiometriaAutenticacionModelInput>(this));
    }
}
