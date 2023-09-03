using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Input;
using Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Output;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.Service.Utils;

namespace Spv.Usuarios.Bff.Service
{
    public class ScoreOperacionesService : IScoreOperacionesService
    {
        private readonly ILogger<ScoreOperacionesService> _logger;
        private readonly IApiScoreOperacionesRepository _scoreOperacionesRepository;
        private readonly IApiUsuariosRepositoryV2 _apiUsuariosRepositoryV2;

        public ScoreOperacionesService(
            ILogger<ScoreOperacionesService> logger,
            IApiScoreOperacionesRepository scoreOperacionesRepository,
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2
        )
        {
            _logger = logger;
            _scoreOperacionesRepository = scoreOperacionesRepository;
            _apiUsuariosRepositoryV2 = apiUsuariosRepositoryV2;
        }

        public async Task<IResponse<ApiScoreOperacionesInicioSesionModelOutput>> InicioSesionAsync(
            string numeroDocumento,
            string nombreUsuario,
            string deviceId
        )
        {
            try
            {
                var usuario = await _apiUsuariosRepositoryV2.ObtenerUsuarioAsync(
                    (numeroDocumento, Base64Operation.Decode(nombreUsuario))
                );

                var inicioSesion = new ApiScoreOperacionesInicioSesionModelInput
                {
                    IdPersona = usuario != null ? usuario.IdPersona : "",
                    CBU = "Desconocido",
                    IdDispositivo = deviceId,
                    Motivo = "Logeado"
                };

                await _scoreOperacionesRepository.InicioSesionAsync(inicioSesion);

                return Responses.Ok(new ApiScoreOperacionesInicioSesionModelOutput());
            }
            catch (Exception ex)
            {
                _logger.LogError(ScoreOperacionesEvents.ExceptionCallingInicioSesionAsync, ex.Message, ex);
                return Responses.Ok(new ApiScoreOperacionesInicioSesionModelOutput());
            }
        }
    }
}
