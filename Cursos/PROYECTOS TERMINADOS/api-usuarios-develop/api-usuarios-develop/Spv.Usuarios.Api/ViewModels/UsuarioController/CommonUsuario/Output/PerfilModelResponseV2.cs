using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// PerfilModelResponse
    /// </summary>
    [JsonObject(Title = "perfil")]
    public class PerfilModelResponseV2
    {
        /// <summary>
        /// Fecha último login
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.UltimoLogin)]
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long? PersonId { get; set; }

        /// <summary>
        /// Id usuario
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdUsuario)]
        public int UserId { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Email)]
        public string Email { get; set; }

        /// <summary>
        /// Nombre 
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Nombre)]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Apellido)]
        public string LastName { get; set; }

        /// <summary>
        /// DocumentType
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.TipoDocumento)]
        public int DocumentType { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Pais)]
        public int Country { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Genero)]
        public string Gender { get; set; }

        /// <summary>
        /// LastPasswordChange
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.FechaUltimoCambioClave)]
        public DateTime? LastPasswordChange { get; set; }

        /// <summary>
        /// PasswordExpiryDate
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.FechaVencimientoClave)]
        public DateTime? PasswordExpiryDate { get; set; }

        /// <summary>
        /// PasswordExpiryDate
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IsEmployee)]
        public bool? IsEmployee { get; set; }

        /// <summary>
        /// PasswordExpiryDate
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.UserStatusId)]
        public int UserStatusId { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilModelResponseV2>> FromAsync(Task<IResponse<PerfilModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static PerfilModelResponseV2 FromGetPerfil(PerfilModelOutput output)
        {
            return FromProfileModelOutput(output);
        }

        private static PerfilModelResponseV2 FromProfileModelOutput(PerfilModelOutput output)
        {
            return new PerfilModelResponseV2
            {
                LastLogon = output.LastLogon,
                PersonId = output.PersonId,
                UserId = output.UserId,
                DocumentNumber = output.DocumentNumber,
                Email = output.Email,
                FirstName = output.FirstName,
                LastName = output.LastName,
                DocumentType = output.DocumentType,
                Gender = output.Gender,
                Country = output.Country,
                LastPasswordChange = output.LastPasswordChange,
                PasswordExpiryDate = output.PasswordExpiryDate,
                IsEmployee = output.IsEmployee,
                UserStatusId = output.UserStatusId
            };
        }
    }
}
