using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.DynamicImagesService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Exceptions;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.Service.Helpers
{
    public static class UsuariosHelper
    {
        /// <summary>
        /// Validar si una determinada persona posee o no Usuario en el nuevo modelo
        /// </summary>
        /// <param name="usuariosRepositoryV2">Inyección de dependencia por parámetros</param>
        /// <param name="personaFisicaModel">Datos de la Persona Física</param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaUsuarioAsync(
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            PersonaModelOutput personaFisicaModel)
        {
            var body = new ApiUsuariosValidacionExistenciaModelInput
            {
                id_pais = personaFisicaModel.PaisDocumento,
                id_tipo_documento = personaFisicaModel.TipoDocumento,
                nro_documento = personaFisicaModel.NumeroDocumento
            };

            var resultExistencia = await usuariosRepositoryV2.ValidarExistenciaAsync(body);

            if (!resultExistencia.IsSuccessStatusCode)
            {
                return await ProcessExternalError<ValidacionExistenciaModelOutput>.ProcessApiUsuariosErrorResponse(resultExistencia);
            }

            var json = await resultExistencia.Content.ReadAsStringAsync();
            var existenciaModelOutput = JsonConvert.DeserializeObject<ApiUsuariosValidacionExistenciaModelOutput>(json);
            
            return Responses.Ok(new ValidacionExistenciaModelOutput
            {
                Migrado = existenciaModelOutput.migrado,
                IdPersona = personaFisicaModel.Id,
                Telefono = personaFisicaModel.TelefonoDobleFactor ? personaFisicaModel.Telefono : string.Empty,
                Usuario = string.Empty, // Se evita enviar el Usuario en este servicio por temas de seguridad CDIG-2894
                IdEstadoUsuario = existenciaModelOutput.id_estado_usuario
            });
        }

        /// <summary>
        /// Devuelve el usuario de una persona por NumeroDocumento,TipoDocumento,PaisDocumento
        /// </summary>
        /// <param name="usuariosRepositoryV2">Inyección de dependencia por parámetros</param>
        /// <param name="perfilModel">Datos del perfil</param>
        /// <returns></returns>
        public static async Task<string> RecuperarUsuarioAsync(
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            ApiUsuariosPerfilModelOutputV2 perfilModel)
        {
            var body = new ApiUsuariosValidacionExistenciaModelInput
            {
                id_pais = perfilModel.pais,
                id_tipo_documento = perfilModel.tipo_documento,
                nro_documento = perfilModel.nro_documento
            };

            var resultExistencia = await usuariosRepositoryV2.ValidarExistenciaAsync(body);

            var json = await resultExistencia.Content.ReadAsStringAsync();
            var existenciaModelOutput = JsonConvert.DeserializeObject<ValidacionExistenciaModelOutput>(json);

            return existenciaModelOutput.Usuario;
        }

        /// <summary>
        /// Validar si una determinada persona posee o no Usuario en el viejo modelo
        /// </summary>
        /// <param name="usuariosRepository">Inyección de dependencia por parámetros</param>
        /// <param name="UserName">Datos del usuario</param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaHbiModelOutput>> ValidarExistenciaUsuarioHbiAsync(
            IApiUsuariosRepository usuariosRepository,
            string UserName)
        {
            var body = new ApiUsuariosValidacionExistenciaHbiModelInput
            {
                user_name = UserName
            };

            var resultExistencia = await usuariosRepository.ValidarExistenciaAsync(body);

            if (!resultExistencia.IsSuccessStatusCode)
            {
                return await ProcessExternalError<ValidacionExistenciaHbiModelOutput>.ProcessApiUsuariosErrorResponse(resultExistencia);
            }

            var json = await resultExistencia.Content.ReadAsStringAsync();
            var existenciaHbiModelOutput = JsonConvert.DeserializeObject<ApiUsuariosValidacionExistenciaHbiModelOutput>(json);

            return Responses.Ok(new ValidacionExistenciaHbiModelOutput
            {
                ExisteUsuario = existenciaHbiModelOutput.existe_usuario
            });
        }

        /// <summary>
        /// ProcessPhoneCreationAsync
        /// </summary>
        /// <param name="function"></param>
        /// <param name="personId"></param>
        /// <param name="phone"></param>
        /// <param name="statusCode"></param>
        /// <param name="messageError"></param>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        public static async Task ProcessPhoneCreationAsync(
            Func<string, ApiPersonasActualizacionTelefonoModelInput, string, Task<ApiPersonasCreacionTelefonoModelOutput>> function,
            string personId,
            string phone,
            HttpStatusCode statusCode,
            string messageError,
            string phoneId)
        {
            if (statusCode != HttpStatusCode.OK)
            {
                switch (statusCode)
                {
                    // Si el resultado de la creación del teléfono es 409-Conflict, realizamos un PATCH para actualizarlo
                    case HttpStatusCode.Conflict:
                        var phoneUpdateBody = new ApiPersonasActualizacionTelefonoModelInput
                        {
                            confiable = true,
                            principal = true,
                            pais = AppConstants.ArgentinaCodigoBantotal,
                            numero = phone
                        };

                        var phoneUpdateResult = await function(personId, phoneUpdateBody, phoneId);

                        // Si no se pudo completar la actualización, falla la registración
                        if (phoneUpdateResult.status_code != HttpStatusCode.OK)
                        {
                            throw new BusinessException(messageError, 0);
                        }
                        break;
                    default:
                        throw new BusinessException(messageError, 0);
                }
            }
        }

        public static ApiNotificacionesEnviarEmailModelInput ArmarEmailBody(
            long personaId,
            string templateId,
            Destinatario destinatario,
            List<VariablesTemplate> variablesTemplate)
        {
            return new ApiNotificacionesEnviarEmailModelInput
            {
                id_persona = personaId,
                modo_envio = AppConstants.EmailModoEnvio,
                destinatarios = new List<Destinatario>
                {
                    destinatario
                },
                template_id = templateId,
                variables_template = variablesTemplate
            };
        }

        public static Destinatario NuevoDestinatario(string email)
        {
            return new Destinatario
            {
                medio = AppConstants.EmailDestinatarioMedio,
                email = email
            };
        }

        public static VariablesTemplate NuevaVariableTemplate(string clave, string valor)
        {
            return new VariablesTemplate { clave = clave, valor = valor };
        }

        public static string OfuscarMail(string email, ILogger<UsuariosService> _logger)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var caracteresSinOfuscar = 2;
                    var emailSemiofuscado = new StringBuilder(email);
                    var indexArroba = email.IndexOf("@");

                    for (int i = caracteresSinOfuscar; i < indexArroba; i++)
                    {
                        emailSemiofuscado[i] = '*';
                    }

                    return emailSemiofuscado.ToString();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(UsuarioServiceEvents.ExceptionCallingRecuperarUsuario, ex.Message, ex);
                return string.Empty;
            }
        }

        public static async Task<ApiUsuariosPerfilModelOutputV2> DeserializarPerfilV2Async(HttpResponseMessage response)
        {
            var usuarioPerfil = new ApiUsuariosPerfilModelOutputV2();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                usuarioPerfil =  JsonConvert.DeserializeObject<ApiUsuariosPerfilModelOutputV2>(json);            
            }

            return usuarioPerfil;
        }

        public static async Task<List<ImagenLoginModelOutput>> DeserializarImagenLoginAsync(HttpResponseMessage response)
        {
            var imagenlogin = new List<ImagenLoginModelOutput>();

            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                imagenlogin = JsonConvert.DeserializeObject<List<ImagenLoginModelOutput>>(json);
            }

            return imagenlogin;
        }
    }
}