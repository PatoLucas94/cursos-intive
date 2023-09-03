using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSBT_WS;
using Spv.Usuarios.Common.Configurations;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.NSBTClient;
using Spv.Usuarios.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.DataAccess.ExternalWebService
{
    public class NsbtRepository : INsbtRepository
    {
        private readonly IOptions<NsbtConfigurationOptions> _nsbtConfigurations;
        private readonly ILogger<NsbtRepository> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public NsbtRepository(IOptions<NsbtConfigurationOptions> nsbtConfigurations, ILogger<NsbtRepository> logger)
        {
            _nsbtConfigurations = nsbtConfigurations;
            _logger = logger;
        }

        /// <summary>
        /// GetPinAsync
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public async Task<PinFromNsbt> GetPinAsync(string countryId, int documentTypeId, string documentNumber)
        {
            var ws = new X9996C1SoapPortClient(
                _nsbtConfigurations.Value.Url ??
                throw new Exception($"No se encontró '{nameof(_nsbtConfigurations.Value.Url)}' key")
            );

            var personParam = GetPersonParameter(countryId, documentTypeId, documentNumber);

            var request = new ExecuteRequest
            {
                Acnco = 308, // canal call-center
                Bopco = 1, // operación
                Cvart = 11, // variante
                Ddpgc = 1, // empresa
                Drqus = _nsbtConfigurations.Value.UsersNSBT ??
                        throw new Exception($"No se encontró '{nameof(_nsbtConfigurations.Value.UsersNSBT)}' key"),
                // datos - tipos - valores
                Edatos = { [0] = "PERSONA", [1] = "ACCION" },
                Etdats = { [0] = "C", [1] = "C" },
                Evalcs = { [0] = personParam, [1] = "1" },
                Ecantlin = 2 // cantidad de parámetros enviados
            };

            var response = await ws.ExecuteAsync(request);

            ExternalServicesCallsLog<NsbtRepository>.LogDebug(
                _logger,
                ExternalServicesNames.NsbtWebService,
                response.Grsco,
                ws.Endpoint.Address.Uri.AbsoluteUri,
                ExternalServicesNames.SoapServiceType,
                JsonSerializer.Serialize(response),
                JsonSerializer.Serialize(request)
            );

            if (response.Grsco != ExecuteResponse.OkResponse) return null;

            var finalResponse = GetPinFromResponse(response);

            return finalResponse;
        }

        public async Task IncrementLoginAttemptsAsync(
            string countryId,
            int documentTypeId,
            string documentNumber,
            string pin,
            int attempt,
            string preservedDateTime = null
        )
        {
            var ws = new X9996C1SoapPortClient(
                _nsbtConfigurations.Value.Url ??
                throw new ApplicationException($"No se encontró '{nameof(_nsbtConfigurations.Value.Url)}' key")
            );

            var personParam = GetPersonParameter(countryId, documentTypeId, documentNumber);
            var attemptsParam = GetAttemptParameter(pin, attempt, preservedDateTime);

            var request = new ExecuteRequest
            {
                Acnco = 308, // canal call-center
                Bopco = 1, // operación
                Cvart = 11, // variante
                Ddpgc = 1, // empresa
                Drqus = _nsbtConfigurations.Value.UsersNSBT ??
                        throw new Exception($"No se encontró '{nameof(_nsbtConfigurations.Value.UsersNSBT)}' key"),
                // datos - tipos - valores
                Edatos = { [0] = "PERSONA", [1] = "ACCION", [2] = "DATOS" },
                Etdats = { [0] = "C", [1] = "C", [2] = "L" },
                Evalcs = { [0] = personParam, [1] = "8" },
                Evalls = { [0] = "0", [1] = "0", [2] = attemptsParam },
                Ecantlin = 3, // cantidad de parámetros enviados
            };

            var response = await ws.ExecuteAsync(request);

            ExternalServicesCallsLog<NsbtRepository>.LogDebug(
                _logger,
                ExternalServicesNames.NsbtWebService,
                response.Grsco,
                ws.Endpoint.Address.Uri.AbsoluteUri,
                ExternalServicesNames.SoapServiceType,
                JsonSerializer.Serialize(response),
                JsonSerializer.Serialize(request)
            );

            if (response.Grsco != ExecuteResponse.OkResponse)
            {
                throw new ApplicationException(
                    MessageConstants.ClaveNumericaErrorWebService
                );
            }
        }

        private static string GetPersonParameter(string countryId, int documentTypeId, string documentNumber)
        {
            var country = countryId;

            if (countryId.Length < 3)
            {
                country = country.PadLeft(country.Length + (3 - country.Length), '0');
            }

            var documentType = documentTypeId.ToString();

            if (documentType.Length == 1)
            {
                documentType = documentType.PadLeft(2, '0');
            }

            return string.Concat(country[^3..], documentType[^2..], documentNumber);
        }

        private static string GetAttemptParameter(string pin, int attempt, string preservedDateTime = null)
        {
            // El servicio espera recibir en este formato los datos del pin válido y
            // dos dígitos utilizados para indicar el número de intentos concatenados a la fecha
            // actual utilizada por el servicio para indicar fecha de ultimo login.
            // Para el caso del pin se destinan 64 posiciones y se completan en blanco
            // a la derecha aquellos lugares que sean necesarios.
            var attemptParam = attempt == 0
                ? $"{pin,-64}{attempt.ToString().PadLeft(2, '0')}{DateTime.Today:yyyyMMdd}"
                : $"{pin,-64}{attempt.ToString().PadLeft(2, '0')}{preservedDateTime}";

            return attemptParam;
        }

        private static PinFromNsbt GetPinFromResponse(ExecuteResponse response)
        {
            if (response.Edatos == null) return null;
            var index = Array.IndexOf(response.Edatos, "PIN");

            if (index <= -1) return null;
            var xmlData = response.Evalls[index];

            var serializer = new XmlSerializer(typeof(PinFromNsbt));

            using TextReader reader = new StringReader(xmlData);
            var result = (PinFromNsbt)serializer.Deserialize(reader);

            return result;
        }
    }
}
