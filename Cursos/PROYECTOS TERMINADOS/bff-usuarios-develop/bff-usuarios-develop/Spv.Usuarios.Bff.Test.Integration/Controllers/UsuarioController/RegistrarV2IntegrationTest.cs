using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.NotificacionesClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class RegistrarV2IntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;
        public WireMockHelper WireMockHelper { get; set; }

        public Dictionary<string, string> ErrorHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "ErrorUser" }
        };

        public Dictionary<string, string> SuccessHeaders = new Dictionary<string, string>
        {
            { HeaderNames.RequestIdHeaderName , "SuccessUser" }
        };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostRegistrarV2(Uri uriBase, RegistracionModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.RegistracionV2);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public RegistrarV2IntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task RegistracionV2CreatedAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse = new ApiPersonasVerificacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                fecha_verificacion = string.Empty,
                id = 1,
                id_telefono = 12,
                usuario_creacion = string.Empty,
                verificado = true
            };

            var ApiUsuariosPerfilInfoOutput =
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = "12345678",
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = 123456
                }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
               .Given(WireMockHelper.Get($"{ApiUsuariosUris.PerfilPathV2(123456)}"))
               .WithTitle("ModificarClaveOkAsync-NoPropia")
               .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.nro_documento == "12345678")));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2CreatedAsync-TyCAceptacion")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), SuccessHeaders))
                .WithTitle("RegistracionV2-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput 
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("RegistracionV2-Created")
                .RespondWith(WireMockHelper.RespondWithCreated());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status201Created);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2ClaveCanalesAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse = new ApiPersonasVerificacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                fecha_verificacion = string.Empty,
                id = 1,
                id_telefono = 12,
                usuario_creacion = string.Empty,
                verificado = true
            };

            var ApiUsuariosPerfilInfoOutput =
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = "12345678",
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = 123456
                }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
               .Given(WireMockHelper.Get($"{ApiUsuariosUris.PerfilPathV2(123456)}"))
               .WithTitle("ModificarClaveOkAsync-NoPropia")
               .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.nro_documento == "12345678")));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2CreatedAsync-TyCAceptacion")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), SuccessHeaders))
                .WithTitle("RegistracionV2-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("RegistracionV2-Created")
                .RespondWith(WireMockHelper.RespondWithCreated());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithUnauthorized());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2BadRequestAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var apiPersonasFisicaInfoExpectedResponse = new ApiPersonasFisicaInfoModelOutput
            {
                id = 123456,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "12345678",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 29140,
                        direccion= "asdfgh@simtlix.com",
                        canal_creacion = "HBI",
                        principal = true,
                        confiable= false
                    }
                }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("400", "", "", "Error de prueba BadRequest", "")
                }
            };

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2BadRequestAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.EmailsCreacionPath(123456)}"))
                .WithTitle("RegistracionV2BadRequestAsync-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonosCreacionPath(123456)}"))
                .WithTitle("RegistracionV2BadRequestAsync-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2BadRequestAsync-PersonasFisicaInfoExpected")
                .RespondWith(WireMockHelper.Json(apiPersonasFisicaInfoExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(path))
                .WithTitle("RegistracionV2BadRequestAsync-BadRequest")
                .RespondWith(WireMockHelper.RespondWithBadRequest(response));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Email = "asdfgh@simtlix.com",
                    Telefono = "3518546564",
                    SmsValidado = false,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2ConflictAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse = new ApiPersonasVerificacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                fecha_verificacion = string.Empty,
                id = 1,
                id_telefono = 12,
                usuario_creacion = string.Empty,
                verificado = true
            };

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("409", "", "", "Error de prueba Conflict", "")
                }
            };

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2ConflictAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonosCreacionPath(123456)}"))
                .WithTitle("RegistracionV2ConflictAsync-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.EmailsCreacionPath(123456)}"))
                .WithTitle("RegistracionV2ConflictAsync-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}"))
                .WithTitle("RegistracionV2ConflictAsync-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2ConflictAsync-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("RegistracionV2ConflictAsync-Conflict")
                .RespondWith(WireMockHelper.RespondWithConflict(response));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status409Conflict);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2ErrorActualizacionEmailAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            IEnumerable<ApiUsuariosActualizarPersonIdModelOutput> ApiUsuariosActualizarPersonIdOutput =
            new List<ApiUsuariosActualizarPersonIdModelOutput>
            {
                            new ApiUsuariosActualizarPersonIdModelOutput
                            {
                                id_persona = 123456
                            }
            };

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2ErrorActualizacionEmailAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", ErrorHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-Error")
                .RespondWith(WireMockHelper.RespondWithConflict(new ApiPersonasCreacionEmailModelOutput { id = 1234 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsActualizacionPath(123456, 1234)}", ErrorHeaders))
                .WithTitle("RegistracionV2-ActualizacionEmail-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            WireMockHelper.ServiceMock
              .Given(WireMockHelper.Patch($"{ApiUsuariosUris.ActualizarPersonId()}"))
              .WithTitle("ActualizarPersonIdAsync")
              .RespondWith(
                  WireMockHelper.Json(ApiUsuariosActualizarPersonIdOutput.First(x => x.id_persona == 123456))
              );

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Email = "test@gmail.com",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(MessageConstants.ApiPersonasErrorActualizacionEmail);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2ErrorCreacionTelefonoDobleFactorAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2ErrorCreacionTelefonoDobleFactorAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2ErrorCreacionTelefonoDobleFactorAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(new ApiTyCAceptacionModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}"))
                .WithTitle("RegistracionV2ErrorCreacionTelefonoDobleFactorAsync-CreacionTelefonoDobleFactor-Error")
                .RespondWith(WireMockHelper.RespondWithConflict(new ApiPersonasCreacionTelefonoModelOutput { id = 1234 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Patch($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}"))
                .WithTitle("RegistracionV2ErrorCreacionTelefonoDobleFactorAsync-ActualizacionTelefonoDobleFactor-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest().WithBodyAsJson(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.EmailsCreacionPath(123456)}"))
                .WithTitle("RegistracionV2ErrorCreacionTelefonoDobleFactorAsync-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Email = "test@gmail.com",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(MessageConstants.ApiPersonasErrorCreacionTelefonoDobleFactor);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2CreatedEnvioMailErrorAsync()
        {
            // Arrange
            const string idTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b";
            var dateTimeNow = DateTime.Now;

            var tycAceptacionResponse = new ApiTyCAceptacionModelOutput
            {
                id_terminos_condiciones = idTyC,
                fecha_aceptacion = dateTimeNow,
                status_code = HttpStatusCode.OK
            };

            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse = new ApiPersonasVerificacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                fecha_verificacion = string.Empty,
                id = 1,
                id_telefono = 12,
                usuario_creacion = string.Empty,
                verificado = true
            };

            var apiPersonasFisicaInfoModelOutputExpectedResponse = new ApiPersonasFisicaInfoModelOutput
            {
                id = 123456,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "12345",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 29140,
                        direccion = "asdfgh@simtlix.com",
                        principal = false,
                        confiable = false,
                        dado_de_baja = false
                    }
                }
            };

            var ApiUsuariosPerfilInfoOutput =
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = "12345678",
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = 123456
                }
            };

            IEnumerable<ApiUsuariosActualizarPersonIdModelOutput> ApiUsuariosActualizarPersonIdOutput =
            new List<ApiUsuariosActualizarPersonIdModelOutput>
            {
                new ApiUsuariosActualizarPersonIdModelOutput
                {
                    id_persona = 123456
                }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
               .Given(WireMockHelper.Get($"{ApiUsuariosUris.PerfilPathV2(123456)}"))
               .WithTitle("ModificarClaveOkAsync-NoPropia")
               .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.nro_documento == "12345678")));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiTyCUris.Aceptados()}"))
                .WithTitle("RegistracionV2CreatedEnvioMailErrorAsync-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(tycAceptacionResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiTyCUris.Aceptados()}", SuccessHeaders))
                .WithTitle("RegistracionV2-AceptacionTyC-OK")
                .RespondWith(WireMockHelper.Json(new ApiTyCAceptacionModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiUsuariosUris.InhabilitacionClaveCanales(), SuccessHeaders))
                .WithTitle("RegistracionV2-InhabilitacionClaveCanales-OK")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2-PersonasFisicaInfo-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasFisicaInfoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), ErrorHeaders))
                .WithTitle("RegistracionV2-EnviarEmail-Error")
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .RespondWith(WireMockHelper.RespondWithCreated());


            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Patch($"{ApiUsuariosUris.ActualizarPersonId()}"))
                .WithTitle("ActualizarPersonIdAsync")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosActualizarPersonIdOutput.First(x => x.id_persona == 123456))
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = "9f825d1c-5d60-45db-8b72-5daecf800a6b"
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status201Created);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RegistracionV2SinTyCCreatedAsync()
        {
            // Arrange
            var apiPersonasVerificacionTelefonoModelOutputExpectedResponse = new ApiPersonasVerificacionTelefonoModelOutput
            {
                canal_creacion = string.Empty,
                fecha_verificacion = string.Empty,
                id = 1,
                id_telefono = 12,
                usuario_creacion = string.Empty,
                verificado = true
            };

            var ApiUsuariosPerfilInfoOutput =
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = "12345678",
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = 123456
                }
            };

            IEnumerable<ApiUsuariosActualizarPersonIdModelOutput> ApiUsuariosActualizarPersonIdOutput =
            new List<ApiUsuariosActualizarPersonIdModelOutput>
            {
                            new ApiUsuariosActualizarPersonIdModelOutput
                            {
                                id_persona = 123456
                            }
            };

            var path = $"{ApiUsuariosUris.RegistracionV2()}";

            SetupCommonWireMocks();

            WireMockHelper.ServiceMock
               .Given(WireMockHelper.Get($"{ApiUsuariosUris.PerfilPathV2(123456)}"))
               .WithTitle("ModificarClaveOkAsync-NoPropia")
               .RespondWith(WireMockHelper.Json(ApiUsuariosPerfilInfoOutput.First(x => x.nro_documento == "12345678")));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonosCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefono-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.EmailsCreacionPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionEmail-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionEmailModelOutput()));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders($"{ApiPersonasUris.TelefonoDobleFactorPath(123456)}", SuccessHeaders))
                .WithTitle("RegistracionV2-CreacionTelefonoDobleFactor-OK")
                .RespondWith(WireMockHelper.Json(new ApiPersonasCreacionTelefonoModelOutput { id = 1 }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiPersonasUris.TelefonoVerificacionPath(1)}"))
                .WithTitle("RegistracionV2-VerificarTelefono-OK")
                .RespondWith(WireMockHelper.Json(apiPersonasVerificacionTelefonoModelOutputExpectedResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(ApiNotificacionesUris.EnviarEmail(), SuccessHeaders))
                .WithTitle("RegistracionV2-EnviarEmail")
                .RespondWith(WireMockHelper.RespondWithAccepted(
                    new ApiNotificacionesEnviarEmailModelOutput
                    {
                        id = 1,
                        estado = "PENDIENTE_ENVIO"
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("RegistracionV2-Created")
                .RespondWith(WireMockHelper.RespondWithCreated());

            WireMockHelper.ServiceMock
                  .Given(WireMockHelper.Patch($"{ApiUsuariosUris.ActualizarPersonId()}"))
                  .WithTitle("ActualizarPersonIdAsync")
                  .RespondWith(
                      WireMockHelper.Json(ApiUsuariosActualizarPersonIdOutput.First(x => x.id_persona == 123456))
                  );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(ApiUsuariosUris.ValidacionClaveCanales()))
                .RespondWith(WireMockHelper.RespondWithAccepted());

            var request = PostRegistrarV2(
                _uriBase,
                new RegistracionModelRequest
                {
                    Clave = "4132",
                    IdPais = 80,
                    IdPersona = "123456",
                    IdTipoDocumento = 4,
                    NroDocumento = "12345678",
                    Usuario = "TestUser0",
                    Telefono = "3518546564",
                    SmsValidado = true,
                    ClaveCanales = "12345678",
                    IdTyC = ""
                });

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status201Created);

            // Clean WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        private void SetupCommonWireMocks()
        {
            var apiUsuariosErrorResponse = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", MessageConstants.DocumentoYaExiste, "")
                }
            };

            var apiPersonasFisicaInfoExpectedResponse = new ApiPersonasFisicaInfoModelOutput
            {
                id = 123456,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "12345678",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 29140,
                        direccion= "asdfgh@simtlix.com",
                        canal_creacion = "HBI",
                        principal = true,
                        confiable= false
                    }
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post($"{ApiUsuariosUris.ValidarExistencia()}"))
                .WithTitle("RegistracionV2Async_ValidarExistencia")
                .RespondWith(WireMockHelper.RespondWithNotFound(apiUsuariosErrorResponse));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(ApiPersonasUris.PersonasFisicaInfo(123456)))
                .WithTitle("RegistracionV2-PersonasFisicaInfoExpected")
                .RespondWith(WireMockHelper.Json(apiPersonasFisicaInfoExpectedResponse));
        }
    }
}
