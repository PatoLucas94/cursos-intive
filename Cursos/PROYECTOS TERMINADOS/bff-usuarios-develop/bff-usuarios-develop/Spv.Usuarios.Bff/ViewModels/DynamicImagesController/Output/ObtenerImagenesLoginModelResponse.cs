using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.DynamicImagesService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.DynamicImagesController.Output
{
    /// <summary>
    /// ObtenerImagenesLoginModelResponses
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ObtenerImagenesLoginModelResponse
    {
        /// <summary>
        /// Imagen
        /// </summary>
        [JsonProperty(PropertyName = "imagen")]
        public byte[] Imagen { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<List<ObtenerImagenesLoginModelResponse>>> FromAsync(
            Task<IResponse<List<ImagenLoginModelOutput>>> task
        )
        {
            var response = await task;
            return response.Map(FromGetImagenLogin);
        }

        private static List<ObtenerImagenesLoginModelResponse> FromGetImagenLogin(List<ImagenLoginModelOutput> output)
        {
            return FromImagenLoginModelOutput(output);
        }

        private static List<ObtenerImagenesLoginModelResponse> FromImagenLoginModelOutput(
            List<ImagenLoginModelOutput> output
        )
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<ImagenLoginModelOutput, ObtenerImagenesLoginModelResponse>());
            var mapper = config.CreateMapper();

            var imagesOutput = mapper.Map<List<ObtenerImagenesLoginModelResponse>>(output);
            return imagesOutput;
        }
    }
}
