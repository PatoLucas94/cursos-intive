using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ValidacionExistenciaModelResponse
    /// </summary>
    public class ValidacionExistenciaModelResponse
    {
        /// <summary>
        /// Constructor ValidacionExistenciaModelResponse
        /// </summary>
        protected ValidacionExistenciaModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long? PersonId { get; set; }

        /// <summary>
        /// Usuario migrado
        /// </summary>
        [JsonProperty(PropertyName = "migrado")]
        public bool Migrated { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [JsonProperty(PropertyName = "usuario")]
        public string Username { get; set; }

        /// <summary>
        /// Id Estado de Usuario
        /// </summary>
        [JsonProperty(PropertyName = "id_estado_usuario")]
        public int UserStatusId { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaModelResponse>> FromAsync(Task<IResponse<ValidacionExistenciaModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromValidarExistencia);
        }

        private static ValidacionExistenciaModelResponse FromValidarExistencia(ValidacionExistenciaModelOutput output)
        {
            return FromValidacionExistenciaModelOutput(output);
        }

        private static ValidacionExistenciaModelResponse FromValidacionExistenciaModelOutput(ValidacionExistenciaModelOutput output)
        {
            return new ValidacionExistenciaModelResponse 
            {
                PersonId = output.PersonId,
                Migrated = output.Migrated,
                Username = output.Username,
                UserStatusId = output.UserStatusId
            };
        }
    }
}
