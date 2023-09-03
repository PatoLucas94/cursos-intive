using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Helpers;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class PersonasService : IPersonasService
    {
        private readonly ILogger<PersonasService> _logger;
        private readonly IApiPersonasRepository _personasRepository;
        private readonly IApiUsuariosRepositoryV2 _usuariosRepositoryV2;
        private readonly IApiCatalogoRepository _catalogoRepository;

        public PersonasService(
            ILogger<PersonasService> logger,
            IApiPersonasRepository personasRepository,
            IApiUsuariosRepositoryV2 usuariosRepositoryV2,
            IApiCatalogoRepository catalogoRepository)
        {
            _logger = logger;
            _personasRepository = personasRepository;
            _usuariosRepositoryV2 = usuariosRepositoryV2;
            _catalogoRepository = catalogoRepository;
        }

        public async Task<IResponse<PersonaModelOutput>> ObtenerPersonaFisicaAsync(IRequestBody<PersonaModelInput> personaModel)
        {
            try
            {
                // Obtenemos Persona
                var persona = await _personasRepository.ObtenerPersonaAsync(
                    personaModel.Body.NumeroDocumento,
                    personaModel.Body.IdTipoDocumento,
                    personaModel.Body.IdPais);

                if (persona == null)
                {
                    return Responses.NotFound<PersonaModelOutput>(ErrorCode.PersonaInexistente.ErrorDescription);
                }

                // Obtenemos Persona Física
                var personaFisica = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, persona.id);

                // Validamos la existencia del usuario
                var resultExistenciaUsuario = await UsuariosHelper.ValidarExistenciaUsuarioAsync(_usuariosRepositoryV2, personaFisica);

                // Si el usuario existe, devolvemos Conflict
                return resultExistenciaUsuario.IsOk ?
                    Responses.Conflict<PersonaModelOutput>(MessageConstants.DocumentoYaExiste) :
                    Responses.Ok(personaFisica);
            }
            catch (Exception ex)
            {
                _logger.LogError(PersonaServiceEvents.ExceptionCallingPersonaFisica, ex.Message, ex);
                throw;
            }
        }

        public async Task<IResponse<PersonaModelOutput>> ObtenerPersonaFiltroAsync(IRequestBody<PersonaFiltroInput> personaModel)
        {
            try
            {
                // Obtenemos Personas
                var personas = await _personasRepository.ObtenerPersonaFiltroAsync(personaModel.Body.NumeroDocumento);

                if (personas == null || personas.Count == 0)
                {
                    return Responses.NotFound<PersonaModelOutput>(ErrorCode.PersonaInexistente.ErrorDescription);
                }

                ApiPersonasFiltroModelOutput persona = null;

                if (personas.Count == 1 ||
                    personaModel.Body.TipoDocumento != null ||
                    personaModel.Body.IdPais != null)
                {
                    persona = personas.Count == 1
                        ? personas.First()
                        : personas.FirstOrDefault(x =>
                            x.tipo_documento == (personaModel.Body.TipoDocumento ?? (int)TipoDocumento.Pasaporte)
                            && x.pais_documento == (personaModel.Body.IdPais ?? AppConstants.ArgentinaCodigoBantotal));
                }

                if (persona != null || PersonasHelper.ExisteConflictoConDniVsLibretas(personaModel.Body.NumeroDocumento, personas))
                {
                    persona ??= personas.First(x => x.tipo_documento != (int)TipoDocumento.Dni);

                    // Obtenemos Persona Física
                    var personaFisica = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, persona.id);

                    // Validamos la existencia del usuario
                    var resultExistenciaUsuario = 
                        await UsuariosHelper.ValidarExistenciaUsuarioAsync(_usuariosRepositoryV2, personaFisica);

                    // Si el usuario existe, devolvemos Conflict
                    return resultExistenciaUsuario.IsOk 
                        ? Responses.Conflict<PersonaModelOutput>(MessageConstants.DocumentoYaExiste) 
                        : Responses.Ok(personaFisica);
                }

                var resultsPersona = new List<PersonaModelOutput>();
                var tiposDocumento = new List<TipoDocumentoModelOutput>();
                var paises = new List<PaisModelOutput>();

                await ComprobarConflictosPersona(personas, resultsPersona, tiposDocumento, paises);

                if (resultsPersona.Count <= 1)
                {
                    return resultsPersona.Count == 1
                        ? Responses.Ok(resultsPersona.FirstOrDefault())
                        : Responses.Conflict<PersonaModelOutput>(MessageConstants.DocumentoYaExiste);
                }

                var conflicto = await PersonasHelper.RecopilarConflictosDePersonas(_catalogoRepository, tiposDocumento, paises);

                var response = new PersonaModelOutput
                {
                    TiposDocumento = conflicto.TiposDocumento,
                    Paises = conflicto.Paises
                };

                return Responses.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(PersonaServiceEvents.ExceptionCallingPersonasFiltro, ex.Message, ex);
                throw;
            }
        }

        private async Task ComprobarConflictosPersona(
            IEnumerable<ApiPersonasFiltroModelOutput> personas, 
            ICollection<PersonaModelOutput> resultsPersona, 
            ICollection<TipoDocumentoModelOutput> tiposDocumento,
            ICollection<PaisModelOutput> paises)
        {
            foreach (var persona in personas)
            {
                // Obtenemos Persona Física
                var personaFisica = await PersonasHelper.GetInfoPersonaFisicaAsync(_personasRepository, persona.id);

                // Validamos la existencia del usuario
                var resultExistenciaUsuario =
                    await UsuariosHelper.ValidarExistenciaUsuarioAsync(_usuariosRepositoryV2, personaFisica);

                if (resultExistenciaUsuario.IsOk) continue;

                resultsPersona.Add(personaFisica);

                tiposDocumento.Add(new TipoDocumentoModelOutput
                {
                    codigo = persona.tipo_documento,
                    descripcion = AppConstants.GetTipoDocumentoDesc(persona.tipo_documento)
                });

                paises.Add(new PaisModelOutput
                {
                    codigo = persona.pais_documento
                });
            }
        }
    }
}
