using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.SoftToken.Input
{
    /// <summary>
    /// SoftTokenValidoModelRequest
    /// </summary>
    public class SoftTokenValidoModelRequest
    {
        /// <summary>
        /// Identificador sys_idPersona
        /// </summary>
        [FromQuery(Name = ParameterNames.Identificador)]
        [JsonProperty(PropertyName = ParameterNames.Identificador)]
        public string Identificador { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [FromQuery(Name = ParameterNames.Token)]
        [JsonProperty(PropertyName = ParameterNames.Token)]
        public string Token { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<SoftTokenValidoModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(new SoftTokenValidoModelInput
            {
                Identificador = Identificador,
                Token = Token
            });
        }
    }
}
