using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Domain.Errors;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Helpers
{
    public static class ProcessExternalError<T>
    {
        #region ApiUsuarios

        public static async Task<IResponse<T>> ProcessApiUsuariosErrorResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiUsuariosErrorResponse>(json);

            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest => GenerateApiUsuariosBadRequestResponse(error),
                HttpStatusCode.Unauthorized => Responses.Unauthorized<T>(error.Errores.First().Detalle,
                    error.Errores.First().Codigo),
                HttpStatusCode.Conflict => Responses.Conflict<T>(error.Errores.First().Detalle,
                    error.Errores.First().Codigo),
                HttpStatusCode.NotFound => Responses.NotFound<T>(error.Errores.First().Detalle,
                    error.Errores.First().Codigo),
                HttpStatusCode.InternalServerError => Responses.InternalServerError<T>(!error.Errores.Any()
                    ? new Exception(MessageConstants.MensajeGenerico)
                    : new Exception(string.Join(". ", error.Errores.Select(s => s.Detalle)))),
                _ => Responses.InternalServerError<T>(new Exception(MessageConstants.MensajeGenerico))
            };
        }

        private static IResponse<T> GenerateApiUsuariosBadRequestResponse(ApiUsuariosErrorResponse apiUsuariosResponse)
        {
            var internalCodeAndDetailErrors = new List<InternalCodeAndDetailErrors>();

            apiUsuariosResponse.Errores.ForEach(error =>
            {
                internalCodeAndDetailErrors.Add(new InternalCodeAndDetailErrors
                    { InternalCode = error.Codigo, Detail = error.Detalle });
            });

            return Responses.BadRequest<T>(internalCodeAndDetailErrors);
        }

        #endregion

        #region ApiNotificaciones

        public static async Task<IResponse<T>> ProcessApiNotificacionesErrorResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiNotificacionesErrorResponse>(json);

            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest => GenerateApiNotificacionesBadRequestResponse(error),
                HttpStatusCode.Unauthorized => Responses.Unauthorized<T>(error.status.ToString(), error.error),
                HttpStatusCode.Conflict => Responses.Conflict<T>(error.error),
                HttpStatusCode.NotFound => Responses.NotFound<T>(error.error),
                HttpStatusCode.InternalServerError => Responses.InternalServerError<T>(new Exception(error.error)),
                _ => Responses.InternalServerError<T>(new Exception(MessageConstants.MensajeGenerico))
            };
        }

        private static IResponse<T> GenerateApiNotificacionesBadRequestResponse(
            ApiNotificacionesErrorResponse apiNotificacionesResponse
        )
        {
            var internalCodeAndDetailErrors = new List<InternalCodeAndDetailErrors>
            {
                new InternalCodeAndDetailErrors
                {
                    InternalCode = apiNotificacionesResponse.status.ToString(), Detail = apiNotificacionesResponse.error
                }
            };

            return Responses.BadRequest<T>(internalCodeAndDetailErrors);
        }

        #endregion
    }
}
