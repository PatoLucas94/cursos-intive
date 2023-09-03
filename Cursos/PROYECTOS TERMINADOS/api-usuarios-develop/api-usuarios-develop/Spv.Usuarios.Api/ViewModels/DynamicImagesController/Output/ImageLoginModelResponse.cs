using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.DynamicImagesService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.DynamicImagesController.Output
{
    /// <summary>
    /// ImageLoginModelResponse
    /// </summary>
    public class ImageLoginModelResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Orden
        /// </summary>
        [JsonProperty(PropertyName = "orden")]
        public int Orden { get; set; }

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
        public static async Task<IResponse<List<ImageLoginModelResponse>>> FromAsync(
            Task<IResponse<List<ImageOutput>>> task
        )
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static List<ImageLoginModelResponse> FromGetPerfil(List<ImageOutput> output)
        {
            return FromProfileModelOutput(output);
        }

        private static List<ImageLoginModelResponse> FromProfileModelOutput(List<ImageOutput> output)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ImageOutput, ImageLoginModelResponse>());
            var mapper = config.CreateMapper();

            var imagesOutput = mapper.Map<List<ImageLoginModelResponse>>(output);
            return imagesOutput;
        }
    }
}
