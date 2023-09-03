using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// RecuperarUsuarioModelResponse
    /// </summary>
    public class RecuperarUsuarioModelResponse
    {
        /// <summary>
        /// EmailSemiOfuscado
        /// </summary>
        [JsonProperty(PropertyName = "EmailSemiOfuscado")]
        public string EmailSemiOfuscado { get; set; }

        /// <summary>
        /// Constructor RecuperarUsuarioModelResponse
        /// </summary>
        protected RecuperarUsuarioModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<RecuperarUsuarioModelResponse>> FromAsync(
            Task<IResponse<ValidacionExistenciaModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<RecuperarUsuarioModelResponse>);
        }
    }
}
