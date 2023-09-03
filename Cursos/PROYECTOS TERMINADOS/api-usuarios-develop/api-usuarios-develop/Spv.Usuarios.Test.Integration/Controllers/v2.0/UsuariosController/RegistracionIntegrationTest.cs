using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Test.Infrastructure;
using Spv.Usuarios.Test.Integration.ExternalServices;
using Xunit;
using static Spv.Usuarios.Common.Testing.Attributes.PriorityAttribute;

namespace Spv.Usuarios.Test.Integration.Controllers.v2._0.UsuariosController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    [TestCaseOrderer("Spv.Usuarios.Common.Testing.PriorityOrderer", "Spv.Usuarios.Common")]
    public class RegistracionIntegrationTest : ControllerIntegrationTest
    {
        private readonly ServerFixture _server;
        private readonly Uri _uriBase;
        private WireMockHelper WireMockHelper { get; }

        private const string NroDocCorrectoV1 = "38502285";

        private readonly ConfiguracionRepository _configuracionesRepository;

        private static ServiceRequest PostRegistracion(Uri uriBase, RegistracionModelRequestV2 registracionModelRequest)
        {
            var uri = new Uri(uriBase, ApiUris.RegistracionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, registracionModelRequest);
        }

        readonly PersonaFisicaInfoModelResponse _personaFisicaInfoModelResponseV1 = new PersonaFisicaInfoModelResponse
        {
            id = 55875268,
            pais_documento = 80,
            tipo_documento = 4,
            numero_documento = NroDocCorrectoV1
        };

        protected override IEnumerable<ServiceRequest> AllRequests => new[]
        {
            PostRegistracion(
                _uriBase, 
                new RegistracionModelRequestV2
                {
                    IdPersona = 1, 
                    IdPais = 80, 
                    IdTipoDocumento = 4, 
                    NroDocumento = "nroDocumento", 
                    Usuario = "usuario", 
                    Clave = "clave"
                })
        };

        private const string NroDocCorrecto = "23734745";
        private const string NroDocIncorrecto = "23734746";

        public RegistracionIntegrationTest(ServerFixture server) : base(server)
        {
            _server = server;
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
            var db = _server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContext>();
            var dbV2 = _server.HttpServer.TestServer.Services.GetRequiredService<GenericDbContextV2>();
            _configuracionesRepository = new ConfiguracionRepository(db);
        }

        [Fact, TestPriority(0)]
        public async Task Registracion()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = NroDocCorrecto,
                Usuario = "Usuario999",
                Clave = "4132"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(1)]
        public async Task Registracion_NoSeEncontroPersonaFisica_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(99999)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.RespondWithNotFound());

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 99999,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = NroDocCorrecto,
                Usuario = "usuario999",
                Clave = "1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(2)]
        public async Task Registracion_NroDocumentoInvalidoDePersonaFisica_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(88888)}";
            _personaFisicaInfoModelResponse.numero_documento = "no es un número";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 88888,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = NroDocCorrecto,
                Usuario = "usuario999",
                Clave = "1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(3)]
        public async Task Registracion_NroDocumentoInvalido_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "nro doc inválido",
                Usuario = "usuario999",
                Clave = "12357891"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(4)]
        public async Task Registracion_NroDocumentoNoCoincide_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = NroDocIncorrecto,
                Usuario = "Usuario999",
                Clave = "Clave1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var content = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.NroDocumentoNoCoincide, NroDocIncorrecto, NroDocCorrecto));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(5)]
        public async Task Registracion_TipoDocumentoNoCoincide_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 80,
                IdTipoDocumento = 5,
                NroDocumento = NroDocCorrecto,
                Usuario = "Usuario999",
                Clave = "Clave1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var content = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.TipoDocumentoNoCoincide, 5, 4));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(6)]
        public async Task Registracion_PaisNoCoincide_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 81,
                IdTipoDocumento = 4,
                NroDocumento = NroDocCorrecto,
                Usuario = "Usuario999",
                Clave = "Clave1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var content = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.PaisNoCoincide, 81, 80));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(7)]
        public async Task Registracion_Inconsistencias_BadRequest()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponse));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 14155916,
                IdPais = 81,
                IdTipoDocumento = 5,
                NroDocumento = NroDocIncorrecto,
                Usuario = "Usuario999",
                Clave = "Clave1235"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var content = await sut.Content.ReadAsAsync<ErrorDetailModel>();

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            content.Should().NotBeNull();
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.NroDocumentoNoCoincide, NroDocIncorrecto, NroDocCorrecto));
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.TipoDocumentoNoCoincide, 5, 4));
            content.Errors[0].Detail.Should().Contain(
                string.Format(MessageConstants.PaisNoCoincide, 81, 80));

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact, TestPriority(8)]
        public async Task Registracion_SinHeaders_BodyCorrecto_BadRequest()
        {
            // Arrange
            _server.HttpServer.HttpClient.DefaultRequestHeaders.Clear();

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "usuario",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact, TestPriority(9)]
        public async Task Registracion_HeadersIncorrectos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add("X-User", "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "usuario",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact, TestPriority(10)]
        public async Task Registracion_HeadersIncompletos_BodyCorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "usuario",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact, TestPriority(11)]
        public async Task Registracion_BodyIncorrecto_BadRequest()
        {
            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "canal");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");
            defaultRequestHeaders.Add(HeaderNames.RequestIdHeaderName, "requestId");

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { username = "xxxxx", password = "yyyyy", documentNumer = "11111" }),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact, TestPriority(12)]
        public async Task RegistracionV1() 
        {
            var config = await _configuracionesRepository.Filter(c => c.Type == AppConstants.ConfigurationTypeDigitalCredentials 
            && c.Name == AppConstants.RegistracionNuevoModeloHabilitado).FirstOrDefaultAsync();

            config.Value = "false";
            _configuracionesRepository.Update(config);
            await _configuracionesRepository.SaveChangesAsync();


            // Arrange
            var path = $"{ApiPersonasUris.PersonaFisicaInfo(55875268)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(_personaFisicaInfoModelResponseV1));

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 55875268,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = NroDocCorrectoV1,
                Usuario = "Usuario999",
                Clave = "Clave4132"
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().NotBeNull();

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        readonly PersonaFisicaInfoModelResponse _personaFisicaInfoModelResponse = new PersonaFisicaInfoModelResponse
        {
            id = 14155916,
            pais_documento = 80,
            tipo_documento = 4,
            numero_documento = NroDocCorrecto,
            emails = new List<Email>
            {
                new Email
                {
                    id = 29140,
                    direccion= "asdfgh@simtlix.com",
                    nombre_interlocutor = null,
                    cargo_interlocutor =  null,
                    fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                    fecha_modificacion = null,
                    canal_creacion = "HBI",
                    canal_modificacion = null,
                    usuario_creacion= "test",
                    usuario_modificacion = null,
                    origen_contacto = "PERSONAL",
                    principal = false,
                    confiable= false,
                    etiquetas = new List<Etiqueta>
                    {
                        new Etiqueta
                        {
                            codigo = "NOTIFICACIONES_MODO",
                            descripcion = "Envío de Notificaciones por MODO"
                        }
                    },
                    dado_de_baja = false
                },
                new Email
                {
                    id = 28934,
                    direccion = "holamundo@gmail.com",
                    nombre_interlocutor = null,
                    cargo_interlocutor = null,
                    fecha_creacion = "2020-09-29 13:14:44.6108690 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8366010 -03:00",
                    canal_creacion = "HBI",
                    canal_modificacion = "BANTOTAL",
                    usuario_creacion = "94415",
                    usuario_modificacion = null,
                    origen_contacto = "PERSONAL",
                    principal = true,
                    confiable = true,
                    etiquetas = new List<Etiqueta>(),
                    dado_de_baja = false
                },
                new Email
                {
                    id = 146,
                    direccion = "ricardo.bertoldo@supervielle.com.ar",
                    nombre_interlocutor = null,
                    cargo_interlocutor = null,
                    fecha_creacion = "2019-12-05 05:00:58.4068810 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8366270 -03:00",
                    canal_creacion = "HBI",
                    canal_modificacion = "BANTOTAL",
                    usuario_creacion = "VOLCADO_MASIVO",
                    usuario_modificacion = null,
                    origen_contacto = null,
                    principal = false,
                    confiable = true,
                    etiquetas = new List<Etiqueta>(),
                    dado_de_baja = false
                }
            },
            tokens_push = new List<TokenPush>
            {
                new TokenPush
                {
                    id = 22687,
                    token = "token_nuevo_de_la_persona_14155917",
                    sistema_operativo = "IOS",
                    fecha_creacion = "2021-02-11 15:14:59.7553090 -03:00",
                    fecha_modificacion = null,
                    canal_creacion = "MBI",
                    canal_modificacion = null,
                    usuario_creacion = null,
                    usuario_modificacion = null

                }
            },
            telefonos = new List<Telefono>
            {
                new Telefono
                {
                    id = 762855,
                    numero_local = 23454356,
                    pais = 80,
                    prefijo_telefonico_pais = 54,
                    codigo_area = 11,
                    numero = "1123454356",
                    interno = null,
                    ddn = "0111523454356",
                    ddi = "+54 9 11 2345 4356",
                    no_llame = "false",
                    fecha_alta_no_llame = null,
                    fecha_baja_no_llame = null,
                    es_geografico = true,
                    nombre_interlocutor = null,
                    cargo_interlocutor = null,
                    score = null,
                    normalizado = true,
                    fecha_creacion = "2021-01-07 15:12:09.1049280 -03:00",
                    fecha_modificacion = null,
                    canal_creacion = "HBI",
                    canal_modificacion = null,
                    usuario_creacion = "18",
                    usuario_modificacion = null,
                    compania = "CLARO",
                    origen_contacto = "PERSONAL",
                    principal = null,
                    confiable = true,
                    doble_factor = null,
                    etiquetas = new List<Etiqueta>(),
                    tipo_telefono = "CELULAR",
                    dado_de_baja = false
                },
                new Telefono
                {
                    id = 625,
                    numero_local = 5550208,
                    pais = 80,
                    prefijo_telefonico_pais = 54,
                    codigo_area = 261,
                    numero = "2615550208",
                    interno = null,
                    ddn = "0261155550208",
                    ddi = "+54 9 261 555 0208",
                    no_llame = "false",
                    fecha_alta_no_llame = null,
                    fecha_baja_no_llame = null,
                    es_geografico = true,
                    nombre_interlocutor = null,
                    cargo_interlocutor = null,
                    score = 0,
                    normalizado = true,
                    fecha_creacion = "2019-12-13 05:01:31.2214160 -03:00",
                    fecha_modificacion = "2021-02-10 15:15:49.6969250 -03:00",
                    canal_creacion = "HBI",
                    canal_modificacion = "HBI",
                    usuario_creacion = "VOLCADO_MASIVO",
                    usuario_modificacion = "18",
                    compania = "CLARO",
                    origen_contacto = null,
                    principal = true,
                    confiable = true,
                    doble_factor = false,
                    etiquetas = new List<Etiqueta>(),
                    tipo_telefono = "CELULAR",
                    dado_de_baja = false
                },
                new Telefono
                {
                    id = 760720,
                    numero_local = 7528992,
                    pais = 80,
                    prefijo_telefonico_pais = 54,
                    codigo_area = 351,
                    numero = "3517528992",
                    interno = null,
                    ddn = "0351157528992",
                    ddi = "+54 9 351 752 8992",
                    no_llame = "false",
                    fecha_alta_no_llame = null,
                    fecha_baja_no_llame = null,
                    es_geografico = true,
                    nombre_interlocutor = null,
                    cargo_interlocutor = null,
                    score = 0,
                    normalizado = true,
                    fecha_creacion = "2020-11-18 17:03:47.2454270 -03:00",
                    fecha_modificacion = "2020-12-14 17:23:03.0498020 -03:00",
                    canal_creacion = "HBI",
                    canal_modificacion = "HBI",
                    usuario_creacion = "test",
                    usuario_modificacion = "test",
                    compania = null,
                    origen_contacto = "PERSONAL",
                    principal = false,
                    confiable = false,
                    doble_factor = false,
                    etiquetas = new List<Etiqueta>
                    {
                        new Etiqueta
                        {
                            codigo = "NOTIFICACIONES_MODO",
                            descripcion = "Envío de Notificaciones por MODO"
                        }
                    },
                    tipo_telefono = "CELULAR",
                    dado_de_baja = false
                }
            },
            domicilios = new List<Domicilio>
            {
                new Domicilio
                {
                    id = 141,
                    fecha_creacion = "2019-12-03 12:04:05.0318220 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8425130 -03:00",
                    canal_creacion = "BANTOTAL",
                    canal_modificacion = "BANTOTAL",
                    usuario_creacion = null,
                    usuario_modificacion = null,
                    pais = 80,
                    provincia = 13,
                    localidad = 548,
                    localidad_maestro = "VILLA NUEVA DE GUAYMALLEN",
                    calle = "CHARCAS",
                    numero = "4116",
                    piso = "",
                    departamento = "",
                    codigo_postal = 5521,
                    codigo_postal_argentino = "M5521NVF",
                    latitud = null,
                    longitud = null,
                    normalizado = true,
                    origen_contacto = null,
                    legal = "true",
                    localidad_alfanumerica = null
                }
            },
            documentos_adicionales = new List<DocumentoAdicional>(),
            actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
            impuestos = new List<Impuesto>
            {
                new Impuesto{
                    id = 212,
                    tipo_impuesto = 1,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 213,
                    tipo_impuesto = 2,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 214,
                    tipo_impuesto = 4,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 215,
                    tipo_impuesto = 5,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 216,
                    tipo_impuesto = 8,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 217,
                    tipo_impuesto = 9,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                    fecha_modificacion = null
                },
                new Impuesto{
                    id = 218,
                    tipo_impuesto = 10,
                    condicion = 1,
                    fecha_creacion = "2019-12-03 12:04:04.8772230 -03:00",
                    fecha_modificacion = null
                }
            },
            tipo_persona = "F",
            fecha_alta_bt = "1998-10-26",
            fecha_baja_bt = null,
            categoria = "N",
            canal_distribucion = 2,
            canal_creacion = "BANTOTAL",
            usuario_creacion = null,
            canal_modificacion = "BANTOTAL",
            usuario_modificacion = null,
            tipo_tributario = 2,
            numero_tributario = "20237347456",
            declaracion_ocde = new DeclaracionOcde
            {
                declara_ocde = false,
                fecha_declaracion = null,
                identificador_unico_ocde = null
            },
            declaracion_fatca = null,
            declaracion_uif = null,
            estado_validacion_documento = "NO_VALIDADO",
            pais_residencia = 10,
            nombre = "RICARDO CRISTIAN",
            apellido = "BERTOLDO",
            genero = "M",
            fecha_nacimiento = "1974-02-10",
            estado_civil = 2,
            paquete = null,
            pais_nacimiento = 80,
            es_ciudadano_legal = true,
            marca_acredita_sueldo = null,
            es_empleado = false,
            registro_patrimonial_matrimonio = null,
            lugar_nacimiento = "ARGENTINA (REPU",
            vinculos_de_personas = new List<VinculoPersona>
            {
                new VinculoPersona
                {
                    canal_creacion = "string",
                    fecha_creacion = "string",
                    id = 0,
                    id_persona_fisica = 0,
                    id_persona_fisica_vinculada = 0,
                    usuario_creacion = "string",
                    vinculo = 0
                }
            },
            estado_persona_expuesta_politicamente = new EstadoPersonaExpuestaPoliticamente
            {
                esta_expuesta = false,
                motivo = null
            },
            fallecido = true,
            fecha_de_fallecimiento = null,
            fecha_de_consulta_renaper = null,
            fecha_creacion = "2019-12-03 12:04:05.0311100 -03:00",
            fecha_modificacion = "2020-10-07 06:44:23.8278290 -03:00",
            es_titular = true
        };

        [Fact, TestPriority(13)]
        public async Task RegistrarValidarReglasCaso1()
        {

            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "OBI");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "1234",
                Clave = "4444a"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact, TestPriority(14)]
        public async Task RegistrarValidarReglasCaso2()
        {

            // Arrange
            var defaultRequestHeaders = _server.HttpServer.HttpClient.DefaultRequestHeaders;
            defaultRequestHeaders.Clear();
            defaultRequestHeaders.Add(HeaderNames.UserHeaderName, "usuario");
            defaultRequestHeaders.Add(HeaderNames.ChannelHeaderName, "OBI");
            defaultRequestHeaders.Add(HeaderNames.ApplicationHeaderName, "aplicacion");

            var uri = new Uri(_uriBase, ApiUris.RegistracionV2);

            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "Userrrr12",
                Clave = "1234"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(registracionModelRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var httpResponseMessage = await _server.HttpServer.HttpClient.PostAsync(uri, content).ConfigureAwait(false);

            var response = await httpResponseMessage.Content.ReadAsAsync<object>();

            // Assert
            httpResponseMessage.Should().NotBeNull();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task RegistracionValidarReglasCaso3()
        {
            var config = await _configuracionesRepository.Filter(c => c.Type == AppConstants.ConfigurationTypeDigitalCredentials
            && c.Name == AppConstants.RegistracionNuevoModeloHabilitado).FirstOrDefaultAsync();

            config.Value = "false";
            _configuracionesRepository.Update(config);
            await _configuracionesRepository.SaveChangesAsync();

            // Arrange
            var registracionModelRequest = new RegistracionModelRequestV2
            {
                IdPersona = 1,
                IdPais = 80,
                IdTipoDocumento = 4,
                NroDocumento = "1111111",
                Usuario = "User",
                Clave = "pass",
            };

            var request = PostRegistracion(_uriBase, registracionModelRequest);

            // Act
            var sut = await SendAsync(request);

            var response = new ObjectResult(await sut.Content.ReadAsAsync<object>());

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

    }
}
