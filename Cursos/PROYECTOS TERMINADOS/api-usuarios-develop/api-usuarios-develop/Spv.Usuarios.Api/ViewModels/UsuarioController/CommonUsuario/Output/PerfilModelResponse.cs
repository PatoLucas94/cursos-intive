using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// PerfilModelResponse
    /// </summary>
    [JsonObject(Title = "perfil")]
    public class PerfilModelResponse
    {
        /// <summary>
        /// Fecha último login
        /// </summary>
        [JsonProperty(PropertyName = "ultimo_login")]
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public string PersonId { get; set; }

        /// <summary>
        /// Id usuario
        /// </summary>
        [JsonProperty(PropertyName = "id_usuario")]
        public int UserId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Nombre 
        /// </summary>
        [JsonProperty(PropertyName = "nombre")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [JsonProperty(PropertyName = "apellido")]
        public string LastName { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilModelResponse>> FromAsync(Task<IResponse<PerfilModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static PerfilModelResponse FromGetPerfil(PerfilModelOutput output)
        {
            return FromProfileModelOutput(output);
        }

        private static PerfilModelResponse FromProfileModelOutput(PerfilModelOutput output)
        {
            return new PerfilModelResponse
            {
                LastLogon = output.LastLogon,
                PersonId = output.PersonId?.ToString(),
                UserId = output.UserId,
                Email = output.Email,
                FirstName = output.FirstName,
                LastName = output.LastName
            };
        }
    }
}
