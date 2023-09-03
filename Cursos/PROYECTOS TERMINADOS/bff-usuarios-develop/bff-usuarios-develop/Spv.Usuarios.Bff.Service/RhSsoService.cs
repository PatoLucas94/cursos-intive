using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.RhSsoService.Output;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Errors;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.Service.Utils;

namespace Spv.Usuarios.Bff.Service
{
    public class RhSsoService : IRhSsoService
    {
        private readonly ILogger<RhSsoService> _logger;
        private readonly IApiUsuariosRepositoryV2 _usuariosRepositoryV2;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public RhSsoService(
            ILogger<RhSsoService> logger,
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            IMapper mapper,
            IBackgroundJobClient backgroundJobClient
        )
        {
            _logger = logger;
            _usuariosRepositoryV2 = usuariosRepositoryV2;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IResponse<TokenModelOutput>> AutenticarAsync(
            IRequestBody<AutenticacionModelInput> autenticacionModel
        )
        {
            try
            {
                var model = _mapper.Map<ApiUsuariosAutenticacionV2ModelInput>(autenticacionModel.Body);
                var response = await _usuariosRepositoryV2.AutenticacionAsync(model);

                if (!response.IsSuccessStatusCode)
                {
                    var jsonError = await response.Content.ReadAsStringAsync();
                    var errorModel = JsonConvert.DeserializeObject<ApiUsuariosErrorResponse>(jsonError);

                    return Responses.BadRequest<TokenModelOutput>(
                        _mapper.Map<List<InternalCodeAndDetailErrors>>(errorModel.Errores)
                    );
                }
                
                var json = await response.Content.ReadAsStringAsync();
                var tokenModel = JsonConvert.DeserializeObject<TokenModelOutput>(json);

                if (string.IsNullOrWhiteSpace(tokenModel.AccessToken))
                    return Responses.Ok(tokenModel);

                _backgroundJobClient.Enqueue<IScoreOperacionesService>(service =>
                    service.InicioSesionAsync(
                        model.nro_documento,
                        Base64Operation.Encode(model.usuario),
                        autenticacionModel.Body.DeviceId)
                );

                _backgroundJobClient.Enqueue<ITyCService>(service =>
                    service.AceptarUsuarioEncriptadoAsync(
                        new ValueTuple<string, string>(model.nro_documento, Base64Operation.Encode(model.usuario))
                    )
                );

                return Responses.Ok(tokenModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, RhSsoServiceEvents.MessageExceptionAutenticar);
                throw;
            }
        }
    }
}
