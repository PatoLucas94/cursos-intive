using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.TyCService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Exceptions;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;
using Spv.Usuarios.Bff.Service.Utils;

namespace Spv.Usuarios.Bff.Service
{
    public class TyCService : ITyCService
    {
        private readonly ILogger<TyCService> _logger;
        private readonly IApiTyCRepository _tycRepository;
        private readonly IApiPersonasRepository _personasRepository;
        private readonly IMapper _mapper;
        private readonly IApiUsuariosRepositoryV2 _apiUsuariosRepositoryV2;

        public TyCService(
            ILogger<TyCService> logger,
            IApiTyCRepository tycRepository,
            IApiPersonasRepository personasRepository,
            IMapper mapper,
            IApiUsuariosRepositoryV2 apiUsuariosRepositoryV2
        )
        {
            _logger = logger;
            _tycRepository = tycRepository;
            _personasRepository = personasRepository;
            _mapper = mapper;
            _apiUsuariosRepositoryV2 = apiUsuariosRepositoryV2;
        }

        public async Task<VigenteModelOutput> ObtenerVigenteAsync()
        {
            try
            {
                var tycVigente = await _tycRepository.ObtenerVigenteAsync();

                if (tycVigente == null)
                    return null;

                var response = new VigenteModelOutput
                {
                    id = tycVigente.id,
                    vigencia_desde = tycVigente.vigencia_desde,
                    contenido = tycVigente.contenido,
                    canales = new List<CanalModelOutput>(),
                    conceptos = new List<ConceptoModelOutput>()
                };

                tycVigente.canales.ForEach(canal =>
                {
                    response.canales.Add(new CanalModelOutput
                    {
                        codigo = canal.codigo,
                        descripcion = canal.descripcion
                    });
                });

                tycVigente.conceptos.ForEach(concepto =>
                {
                    response.conceptos.Add(new ConceptoModelOutput
                    {
                        codigo = concepto.codigo,
                        descripcion = concepto.descripcion
                    });
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TyCServiceEvents.MessageObtenerVigente);
                throw;
            }
        }

        public async Task<IResponse<VigenteModelOutput>> ObtenerVigenteAsync(
            IRequestBody<string> vigenteModel
        )
        {
            try
            {
                var tycVigente = await ObtenerVigenteAsync();

                return tycVigente is null
                    ? Responses.NotFound<VigenteModelOutput>(ErrorCode.TyCVigenteInexistente.ErrorDescription)
                    : Responses.Ok(tycVigente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TyCServiceEvents.MessageObtenerVigente);
                throw;
            }
        }

        public async Task<IResponse<AceptadosModelOutput>> ObtenerAceptadosByPersonIdAsync(
            IRequestBody<string> personId
        )
        {
            try
            {
                var TYCHabilitado = await _apiUsuariosRepositoryV2.ObtenerTYCHabilitadoAsync();

                if (!TYCHabilitado.habilitado)
                    return Responses.NotFound<AceptadosModelOutput>(ErrorCode.TyCNoHabilitado.ErrorDescription);

                var tycVigente = await _tycRepository.ObtenerVigenteAsync();

                if (tycVigente is null)
                    return Responses.NotFound<AceptadosModelOutput>(ErrorCode.TyCVigenteInexistente.ErrorDescription);

                var tycAceptados = await _tycRepository.ObtenerAceptadosByPersonIdAsync(personId.Body);

                if (tycAceptados is null)
                {
                    var persona = await _personasRepository.ObtenerInfoPersonaAsync(personId.Body);

                    return persona?.id is null
                        ? Responses.NotFound<AceptadosModelOutput>(ErrorCode.PersonaInexistente.ErrorDescription)
                        : Responses.Ok(_mapper.Map<AceptadosModelOutput>(tycVigente));
                }

                if (!tycVigente.id.Equals(tycAceptados.id_terminos_condiciones, StringComparison.OrdinalIgnoreCase))
                    return Responses.Ok(_mapper.Map<AceptadosModelOutput>(tycVigente));

                var payload = _mapper.Map<AceptadosModelOutput>(tycVigente);
                payload.aceptados = true;

                return Responses.Ok(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TyCServiceEvents.MessageAceptadosByPersonId);
                throw;
            }
        }

        public async Task<IResponse<AceptadosModelOutput>> ObtenerAceptadosAsync(
            IRequestBody<(string NumeroDocumento, string Usuario)> credenciales
        )
        {
            try
            {
                var TYCHabilitado = await _apiUsuariosRepositoryV2.ObtenerTYCHabilitadoAsync();

                if (!TYCHabilitado.habilitado)
                    return Responses.NotFound<AceptadosModelOutput>(ErrorCode.TyCNoHabilitado.ErrorDescription);

                var tycVigente = await _tycRepository.ObtenerVigenteAsync();

                if (tycVigente is null)
                    return Responses.NotFound<AceptadosModelOutput>(ErrorCode.TyCVigenteInexistente.ErrorDescription);

                var tycAceptados = await _tycRepository.ObtenerAceptadosAsync(credenciales.Body);

                if (tycAceptados is null)
                {
                    var perfil = await _apiUsuariosRepositoryV2.ObtenerUsuarioAsync(credenciales.Body);

                    return perfil?.IdPersona is null
                        ? Responses.NotFound<AceptadosModelOutput>(ErrorCode.PersonaInexistente.ErrorDescription)
                        : Responses.Ok(_mapper.Map<AceptadosModelOutput>(tycVigente));
                }

                if (!tycVigente.id.Equals(tycAceptados.id_terminos_condiciones, StringComparison.OrdinalIgnoreCase))
                    return Responses.Ok(_mapper.Map<AceptadosModelOutput>(tycVigente));

                var payload = _mapper.Map<AceptadosModelOutput>(tycVigente);
                payload.aceptados = true;
                payload.contenido = string.Empty;

                return Responses.Ok(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TyCServiceEvents.MessageAceptadosByPersonId);
                throw;
            }
        }

        public async Task<ApiTyCAceptacionModelOutput> AceptarUsuarioEncriptadoAsync(
            (string NumeroDocumento, string UsuarioEncriptado) credenciales
        )
        {
            credenciales.UsuarioEncriptado = Base64Operation.Decode(credenciales.UsuarioEncriptado);
            return await AceptarAsync(credenciales);
        }

        private async Task<ApiTyCAceptacionModelOutput> AceptarAsync(
            (string NumeroDocumento, string Usuario) credenciales
        )
        {
            try
            {
                var tycVigente = await _tycRepository.ObtenerVigenteAsync();

                if (tycVigente is null)
                    throw new BusinessException(MessageConstants.TyCErrorAceptacion, 0);

                var perfil = await _apiUsuariosRepositoryV2.ObtenerUsuarioAsync(credenciales);

                var tycAceptados = await _tycRepository.AceptarAsync(
                    (Convert.ToInt32(perfil.IdPersona), tycVigente.id)
                );

                return tycAceptados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, TyCServiceEvents.MessageAceptadosByPersonId);
                throw;
            }
        }
    }
}
