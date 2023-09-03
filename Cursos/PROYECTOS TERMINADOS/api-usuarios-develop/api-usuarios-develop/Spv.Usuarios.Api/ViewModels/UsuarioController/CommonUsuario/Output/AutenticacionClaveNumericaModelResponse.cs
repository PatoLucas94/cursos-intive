using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// AutenticacionClaveNumericaModelResponse
    /// </summary>
    public class AutenticacionClaveNumericaModelResponse
    {
        /// <summary>
        /// Constructor AutenticacionClaveNumericaModelResponse
        /// </summary>
        protected AutenticacionClaveNumericaModelResponse()
        {
            /* Empty constructor */
        }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long? PersonId { get; set; }

        /// <summary>
        /// Estado password toma uno de los siguientes valores: EXP -> expirada nuevo modelo / NEXP -> no expirada / EXPBTA -> expirada BTA
        /// </summary>
        [JsonProperty(PropertyName = "estado_password")]
        public string EstadoPassword { get; set; }

        /// <summary>
        /// Fecha de expiración del password
        /// </summary>
        [JsonProperty(PropertyName = "fecha_expiracion_password")]
        public DateTime? FechaExpiracionPassword { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<AutenticacionClaveNumericaModelResponse>> FromAsync(Task<IResponse<AutenticacionClaveNumericaModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromAutenticacionClaveNumerica);
        }

        private static AutenticacionClaveNumericaModelResponse FromAutenticacionClaveNumerica(AutenticacionClaveNumericaModelOutput output)
        {
            return FromAutenticacionModelOutput(output);
        }

        private static AutenticacionClaveNumericaModelResponse FromAutenticacionModelOutput(AutenticacionClaveNumericaModelOutput output)
        {
            return new AutenticacionClaveNumericaModelResponse
            {
                PersonId = output.IdPersona,
                EstadoPassword = output.EstadoPassword,
                FechaExpiracionPassword = output.FechaExpiracionPassword
            };
        }
    }
}
