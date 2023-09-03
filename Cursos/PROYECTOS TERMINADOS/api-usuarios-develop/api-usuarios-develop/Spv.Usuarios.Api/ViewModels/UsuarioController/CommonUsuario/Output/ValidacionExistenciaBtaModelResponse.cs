using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ValidacionExistenciaBtaModelResponse
    /// </summary>
    public class ValidacionExistenciaBtaModelResponse
    {
        /// <summary>
        /// Constructor ValidacionExistenciaBtaModelResponse
        /// </summary>
        protected ValidacionExistenciaBtaModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long? PersonId { get; set; }

        /// <summary>
        /// Usuario registrado (BUU | HBI | NO)
        /// </summary>
        [JsonProperty(PropertyName = "registrado")]
        public string Registrado { get; set; }

        /// <summary>
        /// Usuario tiene clave BT
        /// </summary>
        [JsonProperty(PropertyName = "clave_bt")]
        public bool ClaveBt { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaBtaModelResponse>> FromAsync(Task<IResponse<ValidacionExistenciaBtaModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromValidarExistenciaBta);
        }

        private static ValidacionExistenciaBtaModelResponse FromValidarExistenciaBta(ValidacionExistenciaBtaModelOutput output)
        {
            return FromValidacionExistenciaBtaModelOutput(output);
        }

        private static ValidacionExistenciaBtaModelResponse FromValidacionExistenciaBtaModelOutput(ValidacionExistenciaBtaModelOutput output)
        {
            return new ValidacionExistenciaBtaModelResponse
            {
                PersonId = output.PersonId,
                Registrado = output.Registrado,
                ClaveBt = output.ClaveBt
            };
        }
    }
}
