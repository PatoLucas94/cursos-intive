using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Input;
using Spv.Usuarios.Bff.Common.Dtos.Service.BiometriaService.Output;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Common.LogEvents;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Errors;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.Service.Interface;

namespace Spv.Usuarios.Bff.Service
{
    public class BiometriaService : IBiometriaService
    {
        private readonly ILogger<BiometriaService> _logger;
        private readonly IApiBiometriaRepository _apiBiometriaRepository;
        private readonly IMapper _mapper;

        public BiometriaService(
            ILogger<BiometriaService> logger,
            IApiBiometriaRepository apiBiometriaRepository,
            IMapper mapper
        )
        {
            _logger = logger;
            _apiBiometriaRepository = apiBiometriaRepository;
            _mapper = mapper;
        }

        public async Task<IResponse<BiometriaAutenticacionModelOutput>> AutenticarAsync(
            IRequestBody<BiometriaAutenticacionModelInput> autenticacionModel
        )
        {
            try
            {
                var model = autenticacionModel.Body;
                var response = await _apiBiometriaRepository.AutenticacionAsync(model);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        return Responses.Error<BiometriaAutenticacionModelOutput>(
                            response.StatusCode,
                            $"{ExternalServicesNames.ApiBiometria} - {response.ReasonPhrase}."
                        );
                    }

                    var error = JsonConvert.DeserializeObject<ApiBiometriaErrorResponse>(content);

                    return Responses.Error<BiometriaAutenticacionModelOutput>(
                        error.Codigo,
                        _mapper.Map<List<InternalCodeAndDetailErrors>>(error.Errores)
                    );
                }

                var bioModel = JsonConvert.DeserializeObject<BiometriaAutenticacionModelOutput>(content);

                return Responses.Ok(bioModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, BiometriaServiceEvents.MessageExceptionAutenticar);
                throw;
            }
        }
    }
}
