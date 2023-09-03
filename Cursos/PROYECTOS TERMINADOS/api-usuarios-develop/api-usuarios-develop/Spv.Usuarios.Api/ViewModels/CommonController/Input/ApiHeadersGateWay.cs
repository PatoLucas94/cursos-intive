using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.CommonController.Input
{
    /// <summary>
    /// ApiHeadersGateWay
    /// </summary>
    public class ApiHeadersGateWay : ApiHeaders
    {
        /// <summary>
        /// Gateway
        /// </summary>
        [FromHeader(Name = HeaderNames.GateWay)]
        public string XGateway { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="allowedChannels"></param>
        /// <returns></returns>
        public override IRequestBody<T> ToRequestBody<T>(T body, IAllowedChannels allowedChannels)
        {
            base.ToRequestBody(body, allowedChannels);

            return new RequestBody<T>(XRequestId, XCanal, XUsuario, XAplicacion, XGateway, body);
        }
    }
}
