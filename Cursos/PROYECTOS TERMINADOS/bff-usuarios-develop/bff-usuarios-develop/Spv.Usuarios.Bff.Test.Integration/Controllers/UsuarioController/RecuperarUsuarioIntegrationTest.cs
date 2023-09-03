using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class RecuperarUsuarioIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        private static readonly string nroDocumentoPersonaCorrecta = "23734745";
        private static readonly string nroDocumentoDuplicadoConflictoTipoDoc = "4078985";
        private static readonly string nroDocumentoInexistente = "11111111";
        private static readonly int idPersonaCorrecta = 14155917;
        private static readonly int idPaisPersonaCorrecta = 80;
        private static readonly int idTipoDocumentoPersonaCorrecta = (int)TipoDocumento.Dni;

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    nroDocumentoPersonaCorrecta,
                    "rcbertoldo1",
                    HttpStatusCode.OK
                }
            };

        private static IEnumerable<ApiPersonasFiltroModelOutput> ApiPersonasFiltroModelOutput =>
            new List<ApiPersonasFiltroModelOutput>
            {
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaCorrecta,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "23734745",
                    emails = new List<EmailExtraInfo>
                    {
                        new EmailExtraInfo
                        {
                            id = 29140,
                            direccion = "asdfgh@simtlix.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "HBI",
                            canal_modificacion = null,
                            usuario_creacion = "test",
                            usuario_modificacion = null,
                            origen_contacto = "PERSONAL",
                            principal = false,
                            confiable = false,
                            etiquetas = new List<Etiqueta>
                            {
                                new Etiqueta
                                {
                                    codigo = "NOTIFICACIONES_MODO",
                                    descripcion = "Envío de Notificaciones por MODO"
                                }
                            },
                            dado_de_baja = false
                        }
                    }
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaCorrecta,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "4078985",
                    emails = new List<EmailExtraInfo>
                    {
                        new EmailExtraInfo
                        {
                            id = 29140,
                            direccion = "asdfgh@simtlix.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "HBI",
                            canal_modificacion = null,
                            usuario_creacion = "test",
                            usuario_modificacion = null,
                            origen_contacto = "PERSONAL",
                            principal = false,
                            confiable = false,
                            etiquetas = new List<Etiqueta>
                            {
                                new Etiqueta
                                {
                                    codigo = "NOTIFICACIONES_MODO",
                                    descripcion = "Envío de Notificaciones por MODO"
                                }
                            },
                            dado_de_baja = false
                        }
                    }
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaCorrecta,
                    pais_documento = 80,
                    tipo_documento = 5,
                    numero_documento = "4078985",
                    emails = new List<EmailExtraInfo>
                    {
                        new EmailExtraInfo
                        {
                            id = 29140,
                            direccion = "asdfgh@simtlix.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "HBI",
                            canal_modificacion = null,
                            usuario_creacion = "test",
                            usuario_modificacion = null,
                            origen_contacto = "PERSONAL",
                            principal = false,
                            confiable = false,
                            etiquetas = new List<Etiqueta>
                            {
                                new Etiqueta
                                {
                                    codigo = "NOTIFICACIONES_MODO",
                                    descripcion = "Envío de Notificaciones por MODO"
                                }
                            },
                            dado_de_baja = false
                        }
                    }
                }
            };

        private static IEnumerable<ApiUsuariosPerfilModelOutputV2> ApiUsuariosPerfilOutput =>
            new List<ApiUsuariosPerfilModelOutputV2>
            {
                new ApiUsuariosPerfilModelOutputV2
                {
                    nro_documento = nroDocumentoPersonaCorrecta,
                    tipo_documento = 1,
                    email = "mail@mail.com",
                    id_persona = idPersonaCorrecta
                }
            };

        private static IEnumerable<ApiUsuariosActualizarPersonIdModelOutput> ApiUsuariosActualizarPersonIdOutput =>
            new List<ApiUsuariosActualizarPersonIdModelOutput>
            {
                new ApiUsuariosActualizarPersonIdModelOutput
                {
                    id_persona = idPersonaCorrecta
                }
            };

        private static IEnumerable<ApiPersonasFisicaInfoModelOutput> ApiPersonaFisicaInfoOutput =>
            new List<ApiPersonasFisicaInfoModelOutput>
            {
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaCorrecta,
                    pais_documento = idPaisPersonaCorrecta,
                    tipo_documento = idTipoDocumentoPersonaCorrecta,
                    numero_documento = nroDocumentoPersonaCorrecta,
                    emails = new List<Email>
                    {
                        new Email
                        {
                            id = 29140,
                            direccion = "asdfgh@simtlix.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2020-11-18 17:03:45.9769930 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "HBI",
                            canal_modificacion = null,
                            usuario_creacion = "test",
                            usuario_modificacion = null,
                            origen_contacto = "PERSONAL",
                            principal = false,
                            confiable = false,
                            etiquetas = new List<Etiqueta>
                            {
                                new Etiqueta
                                {
                                    codigo = "NOTIFICACIONES_MODO",
                                    descripcion = "Envío de Notificaciones por MODO"
                                }
                            },
                            dado_de_baja = false
                        }
                    }
                }
            };

        private WireMockHelper WireMockHelper { get; set; }

        public RecuperarUsuarioIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostRecuperarUsuario(Uri uriBase, RecuperarUsuarioModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.RecuperarUsuario);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task RecuperarUsuarioAsync(
            string nroDocumento,
            string usuario,
            HttpStatusCode statusCodeValidarExistencia
        )
        {
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta)}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathPersonaFisica = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathActualizarPersonId = $"{ApiUsuariosUris.ActualizarPersonId()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ApiUsuariosValidacionExistenciaModelOutput { migrado = false }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(
                    WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        new List<ApiPersonasFiltroModelOutput>
                        {
                            ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta)
                        }
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("RecuperarUsuarioAsync")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta))
                );

            WireMockHelper.ServiceMock
            .Given(WireMockHelper.Patch(pathActualizarPersonId))
            .WithTitle("ActualizarPersonIdAsync")
            .RespondWith(
                WireMockHelper.Json(new ApiUsuariosActualizarPersonIdModelOutput
                {
                    id_persona = idPersonaCorrecta
                })
            );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumento}_{statusCodeValidarExistencia}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ValidacionExistenciaModelOutput { Usuario = usuario }));

            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest
                {
                    IdPais = 80,
                    IdTipoDocumento = 4,
                    NroDocumento = "23734745"
                }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task RecuperarUsuarioAsyncAmbigua(
            string nroDocumento,
            string usuario,
            HttpStatusCode statusCodeValidarExistencia
        )
        {
            var pathPersonaDuplicada = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathPersonaFisica = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ApiUsuariosValidacionExistenciaModelOutput { migrado = false }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaDuplicada))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonasFiltroModelOutput.Where(x =>
                                x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc
                            )
                            .ToList()
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("RecuperarUsuarioAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumento}_{statusCodeValidarExistencia}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                        new ValidacionExistenciaModelOutput { Usuario = usuario }
                    )
                );

            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest { NroDocumento = "4078985" }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task RecuperarUsuarioAsyncPersonaDesambiguada(
            string nroDocumento,
            string usuario,
            HttpStatusCode statusCodeValidarExistencia
        )
        {
            var pathPersonaDuplicada = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPerfilV2 = $"{ApiUsuariosUris.PerfilPathV2(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathPersonaFisica = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathActualizarPersonId = $"{ApiUsuariosUris.ActualizarPersonId()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                        new ApiUsuariosValidacionExistenciaModelOutput { migrado = false }
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(
                    WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaDuplicada))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonasFiltroModelOutput.Where(x =>
                                x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc
                            )
                            .ToList()
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPerfilV2))
                .WithTitle("RecuperarUsuarioAsyncPersonaDesambiguada")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosPerfilOutput.First(x => x.id_persona == idPersonaCorrecta))
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumento}_{statusCodeValidarExistencia}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ValidacionExistenciaModelOutput { Usuario = usuario }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Patch(pathActualizarPersonId))
                .WithTitle("ActualizarPersonIdAsync")
                .RespondWith(
                    WireMockHelper.Json(ApiUsuariosActualizarPersonIdOutput.First(x => x.id_persona == idPersonaCorrecta))
                );

            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest
                {
                    IdPais = 80,
                    IdTipoDocumento = 4,
                    NroDocumento = "4078985"
                }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status200OK);

            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncPersonaInexistente()
        {
            var pathPersonaDuplicada = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoInexistente)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathPersonaFisica = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ApiUsuariosValidacionExistenciaModelOutput { migrado = false }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaDuplicada))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonasFiltroModelOutput.Where(x =>
                                x.numero_documento == nroDocumentoInexistente
                            )
                            .ToList()
                    )
                );


            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest { NroDocumento = "4078985" }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task RecuperarUsuarioAsyncPersonaNoDesambiguada(
            string nroDocumento,
            string usuario,
            HttpStatusCode statusCodeValidarExistencia
        )
        {
            var pathPersonaDuplicada = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaDuplicada))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonasFiltroModelOutput.Where(x =>
                                x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc
                            )
                            .ToList()
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("RecuperarUsuarioAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumento}_{statusCodeValidarExistencia}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ValidacionExistenciaModelOutput { Usuario = usuario }));

            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest
                {
                    IdPais = 90,
                    IdTipoDocumento = 2,
                    NroDocumento = "4078985"
                }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task RecuperarUsuarioAsyncThrowException()
        {
            var pathPersonaDuplicada = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathPersonaFisica = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(new ApiUsuariosValidacionExistenciaModelOutput { migrado = false }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisica))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(
                        ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)
                    )
                );

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaDuplicada))
                .WithTitle("RecuperarUsuarioAsync-PersonaCorrecta")
                .RespondWith(
                    WireMockHelper.Json(
                        ApiPersonasFiltroModelOutput.Where(x =>
                                x.numero_documento == nroDocumentoPersonaCorrecta
                            )
                            .ToList()
                    )
                );

            var request = PostRecuperarUsuario(
                _uriBase,
                new RecuperarUsuarioModelRequest { NroDocumento = "23734745" }
            );

            // Act
            var sut = await SendAsync(request, "SuccessUser");

            var response = new ApiUsuariosErrorResponse
            {
                Codigo = 404,
                Errores = new List<ApiUsuarioError> { new ApiUsuarioError("404", "", "", "Perfil inexistente.", "") }
            };

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(response.Errores.First().Detalle);
            result.Errores.First().Codigo.Should().Be(response.Errores.First().Codigo);
        }
    }
}
