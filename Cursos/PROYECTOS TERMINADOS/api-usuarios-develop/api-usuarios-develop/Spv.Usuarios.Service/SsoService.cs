using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Common.Errors;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.DataAccess.Interface.V2;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Service.Helpers;
using Spv.Usuarios.Service.Interface;
using Spv.Usuarios.Service.Utils;

namespace Spv.Usuarios.Service
{
    public class SsoService : ISsoService
    {
        private readonly ILogger<SsoService> _logger;
        private readonly ISsoRepository _ssoRepository;
        private readonly IAuditoriaLogV2Repository _auditoriaV2Repository;
        private readonly IHelperDbServerV2 _helperDbServerV2;
        private readonly IDistributedCache _distributedCache;

        public SsoService(
            ILogger<SsoService> logger,
            ISsoRepository ssoRepository,
            IAuditoriaLogV2Repository auditoriaV2Repository,
            IHelperDbServerV2 helperDbServerV2,
            IDistributedCache distributedCache
        )
        {
            _logger = logger;
            _ssoRepository = ssoRepository;
            _auditoriaV2Repository = auditoriaV2Repository;
            _helperDbServerV2 = helperDbServerV2;
            _distributedCache = distributedCache;
        }

        public async Task<IResponse<TokenModelOutput>> AutenticarAsync(
            IRequestBody<AutenticacionModelInput> autenticacionModel
        )
        {
            try
            {
                var userEncodeLowerCase = UserEncoding.Base64Encode(
                    autenticacionModel.Body.DocumentNumber,
                    autenticacionModel.Body.UserName,
                    autenticacionModel.XCanal,
                    true
                );
                var userEncode = UserEncoding.Base64Encode(
                    autenticacionModel.Body.DocumentNumber,
                    autenticacionModel.Body.UserName,
                    autenticacionModel.XCanal
                );

                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var keyUsuario = DistributedCache.Sso.Autenticacion(userEncodeLowerCase);

                await _distributedCache.SetAsync(
                    keyUsuario,
                    DistributedCache.Serialize(userEncode),
                    DistributedCache.SlidingExpirationMinutes(1)
                );

                var (response, error) = await _ssoRepository.GetTokenAsync(
                    autenticacionModel.Body.DocumentNumber,
                    autenticacionModel.Body.UserName,
                    autenticacionModel.Body.Password,
                    autenticacionModel.XCanal
                );

                await _distributedCache.RemoveAsync(keyUsuario);

                if (error is null)
                {
                    await _auditoriaV2Repository.SaveOkAuditLogAsync(
                        EventTypes.AuthenticationKeycloak,
                        autenticacionModel.XCanal,
                        dateTimeV2,
                        JsonSerializer.Serialize(response)
                    );

                    return Responses.Ok(response);
                }

                var errorCache = await _distributedCache.GetAsync(
                    DistributedCache.Usuario.Error(autenticacionModel.Body.Hash())
                );

                var errorUsuarioService = DistributedCache.Deserialize<AutenticacionModelOutput>(errorCache);

                if (errorUsuarioService is null)
                {
                    // Validando errores al obtener usuario
                    var keyObtenerUsuario = DistributedCache.Usuario.ObtenerUsuario(userEncode);

                    errorCache = await _distributedCache.GetAsync(keyObtenerUsuario);

                    var errorCode = DistributedCache.Deserialize<ErrorCode>(errorCache);

                    if (errorCode is { })
                    {
                        await _distributedCache.RemoveAsync(keyObtenerUsuario);

                        await _auditoriaV2Repository.SaveErrorAuditLogAsync(
                            EventTypes.AuthenticationKeycloak,
                            autenticacionModel.XCanal,
                            dateTimeV2,
                            JsonSerializer.Serialize(errorCode)
                        );

                        return Responses.Unauthorized<TokenModelOutput>(
                            errorCode.Code,
                            errorCode.ErrorDescription
                        );
                    }

                    await _auditoriaV2Repository.SaveErrorAuditLogAsync(
                        EventTypes.AuthenticationKeycloak,
                        autenticacionModel.XCanal,
                        dateTimeV2,
                        JsonSerializer.Serialize(error)
                    );

                    return Responses.Unauthorized<TokenModelOutput>(
                        "SSO",
                        $"{error.Error}. {error.ErrorDescription}."
                    );
                }

                await _distributedCache.RemoveAsync(DistributedCache.Usuario.Error(autenticacionModel.Body.Hash()));

                await _auditoriaV2Repository.SaveErrorAuditLogAsync(
                    EventTypes.AuthenticationKeycloak,
                    autenticacionModel.XCanal,
                    dateTimeV2,
                    JsonSerializer.Serialize(errorUsuarioService)
                );

                return Responses.Unauthorized<TokenModelOutput>(
                    errorUsuarioService.Error?.Code,
                    errorUsuarioService.Error?.ErrorDescription
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UsuarioServiceEvents.CallingAutenticacionMessage);
                throw;
            }
        }

        public async Task<IResponse<TokenModelOutput>> RefreshAccessTokenAsync(IRequestBody<string> tokenModel)
        {
            try
            {
                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var (response, error) = await _ssoRepository.RefreshAccessTokenAsync(tokenModel.Body);

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    0,
                    EventTypes.RefreshAccessKeycloak,
                    response != null ? EventResults.Ok : EventResults.Error,
                    tokenModel.XCanal,
                    dateTimeV2,
                    response != null ? JsonSerializer.Serialize(response) : JsonSerializer.Serialize(error)
                );

                return error != null
                    ? Responses.Unauthorized<TokenModelOutput>("SSO", $"{error.Error}. {error.ErrorDescription}.")
                    : Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UsuarioServiceEvents.CallingAutenticacionMessage);
                throw;
            }
        }

        public async Task<IResponse<IReadOnlyDictionary<string, object>>> VerificarTokenAsync(
            IRequestBody<string> tokenModel
        )
        {
            try
            {
                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var (response, error) = await _ssoRepository.GetIntrospectAsync(tokenModel.Body);

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    0,
                    EventTypes.IntrospectKeycloak,
                    response != null ? EventResults.Ok : EventResults.Error,
                    tokenModel.XCanal,
                    dateTimeV2,
                    response != null ? JsonSerializer.Serialize(response) : JsonSerializer.Serialize(error)
                );

                return error != null
                    ? Responses.BadRequest<IReadOnlyDictionary<string, object>>(
                        $"{error.Error}. {error.ErrorDescription}."
                    )
                    : Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UsuarioServiceEvents.CallingVerificarTokenMessage);
                throw;
            }
        }

        public async Task<IResponse<bool>> CerrarSesionAsync(IRequestBody<string> tokenModel)
        {
            try
            {
                var dateTimeV2 = await _helperDbServerV2.ObtenerFechaAsync();
                var (response, error) = await _ssoRepository.GetLogoutAsync(tokenModel.Body);

                await _auditoriaV2Repository.SaveAuditLogAsync(
                    0,
                    EventTypes.IntrospectKeycloak,
                    error is null ? EventResults.Ok : EventResults.Error,
                    tokenModel.XCanal,
                    dateTimeV2,
                    error is null ? $"Logout: {tokenModel.Body}" : JsonSerializer.Serialize(error)
                );

                return error != null
                    ? Responses.BadRequest<bool>($"{error.Error}. {error.ErrorDescription}.")
                    : Responses.NoContent(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UsuarioServiceEvents.CallingCerrarSesionMessage);
                throw;
            }
        }
    }
}
