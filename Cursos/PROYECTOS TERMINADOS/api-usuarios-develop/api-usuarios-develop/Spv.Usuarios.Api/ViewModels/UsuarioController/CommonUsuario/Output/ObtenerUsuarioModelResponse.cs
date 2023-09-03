using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ObtenerUsuarioModelResponse
    /// </summary>
    public class ObtenerUsuarioModelResponse
    {
        /// <summary>
        /// Constructor ObtenerUsuarioModelResponse
        /// </summary>
        protected ObtenerUsuarioModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public string PersonId { get; set; }

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
        /// Usuario migrado
        /// </summary>
        [JsonProperty(PropertyName = "migrado")]
        public bool Migrated { get; set; }

        /// <summary>
        /// Identifier
        /// </summary>
        [JsonProperty(PropertyName = "identificador")]
        public string Identifier { get; set; }
        
        /// <summary>
        /// Canal
        /// </summary>
        [JsonProperty(PropertyName = "canal")]
        public string Canal { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<ObtenerUsuarioModelResponse>> FromAsync(
            Task<IResponse<PerfilModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<ObtenerUsuarioModelResponse>);
        }
    }
}
