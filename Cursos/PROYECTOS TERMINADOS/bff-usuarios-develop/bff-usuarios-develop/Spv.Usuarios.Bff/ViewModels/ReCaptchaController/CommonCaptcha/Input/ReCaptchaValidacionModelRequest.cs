using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Input
{
    /// <summary>
    /// ReCaptchaValidacionModelRequest
    /// </summary>
    public class ReCaptchaValidacionModelRequest
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        [Required(ErrorMessage = "El campo Token es obligatorio.")]
        public string Token { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        [Required(ErrorMessage = "El campo Action es obligatorio.")]
        public string Action { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<ReCaptchaValidacionModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(
                new ReCaptchaValidacionModelInput
                {
                    Token = Token,
                    Action = Action
                });
        }
    }
}
