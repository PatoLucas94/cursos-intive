using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.CommonController.Input
{
    /// <summary>
    /// ApiHeadersUpdateCredentials
    /// </summary>
    public class ApiHeadersDeviceId : ApiHeaders
    {
        /// <summary>
        /// DeviceId
        /// </summary>
        [FromHeader(Name = HeaderNames.DeviceId)]
        public string DeviceId { get; set; }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        public override IRequestBody<T> ToRequestBody<T>(T body)
        {
            base.ToRequestBody(body);

            return new RequestBody<T>(XRequestId, body, DeviceId);
        }
    }
}
