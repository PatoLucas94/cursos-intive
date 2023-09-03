using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.ReCaptchaService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.ReCaptchaController.CommonCaptcha.Output
{
    /// <summary>
    /// ReCaptchaValidacionModelResponse
    /// </summary>
    public class ReCaptchaValidacionModelResponse
    {
        /// <summary>
        /// Score
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        private static ReCaptchaValidacionModelResponse FromPostValidacionCaptcha(ReCaptchaValidacionModelOutput output)
        {
            return FromValidacionCaptchaModelOutput(output);
        }

        private static ReCaptchaValidacionModelResponse FromValidacionCaptchaModelOutput(ReCaptchaValidacionModelOutput output)
        {
            return new ReCaptchaValidacionModelResponse
            {
                Score = output.Score,
                Success = output.Success
            };
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ReCaptchaValidacionModelResponse>> FromAsync(Task<IResponse<ReCaptchaValidacionModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromPostValidacionCaptcha);
        }
    }
}
