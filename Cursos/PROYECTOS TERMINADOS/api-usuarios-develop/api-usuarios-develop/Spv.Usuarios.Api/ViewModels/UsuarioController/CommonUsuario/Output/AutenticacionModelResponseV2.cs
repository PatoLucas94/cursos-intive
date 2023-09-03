using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// AutenticacionModelResponseV2
    /// </summary>
    public class AutenticacionModelResponseV2
    {
        /// <summary>
        /// Constructor AutenticacionModelResponseV2
        /// </summary>
        protected AutenticacionModelResponseV2()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long? PersonId { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<AutenticacionModelResponseV2>> FromAsync(Task<IResponse<AutenticacionModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetPersonId);
        }

        private static AutenticacionModelResponseV2 FromGetPersonId(AutenticacionModelOutput output)
        {
            return FromAutenticacionModelOutput(output);
        }

        private static AutenticacionModelResponseV2 FromAutenticacionModelOutput(AutenticacionModelOutput output)
        {
            return new AutenticacionModelResponseV2
            {
                PersonId = output.IdPersona
            };
        }
    }
}
