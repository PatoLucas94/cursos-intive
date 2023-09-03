using System;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// PerfilMigradoModelResponse
    /// </summary>
    public class PerfilMigradoModelResponse
    {
        /// <summary>
        /// UserId
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// PersonId
        /// </summary>
        [JsonProperty(PropertyName = "person_id")]
        public long? PersonId { get; set; }

        /// <summary>
        /// DocumentCountryId
        /// </summary>
        [JsonProperty(PropertyName = "document_country_id")]
        public int DocumentCountryId { get; set; }

        /// <summary>
        /// DocumentTypeId
        /// </summary>
        [JsonProperty(PropertyName = "document_type_id")]
        public int DocumentTypeId { get; set; }

        /// <summary>
        /// DocumentNumber
        /// </summary>
        [JsonProperty(PropertyName = "document_number")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// UserStatusId
        /// </summary>
        [JsonProperty(PropertyName = "user_status_id")]
        public byte UserStatusId { get; set; }

        /// <summary>
        /// LoginAttempts
        /// </summary>
        [JsonProperty(PropertyName = "login_attempts")]
        public int? LoginAttempts { get; set; }

        /// <summary>
        /// LastPasswordChange
        /// </summary>
        [JsonProperty(PropertyName = "last_password_change")]
        public DateTime? LastPasswordChange { get; set; }

        /// <summary>
        /// LastLogon
        /// </summary>
        [JsonProperty(PropertyName = "last_logon")]
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        [JsonProperty(PropertyName = "created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilMigradoModelResponse>> FromAsync(
            Task<IResponse<PerfilMigradoModelOutput>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<PerfilMigradoModelResponse>);
        }

        /// <summary>
        /// FromListAsync
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilMigradoModelResponse[]>> FromListAsync(
            Task<IResponse<PerfilMigradoModelOutput[]>> task,
            IMapper mapper
        )
        {
            var response = await task;
            return response.Map(mapper.Map<PerfilMigradoModelResponse[]>);
        }
    }
}