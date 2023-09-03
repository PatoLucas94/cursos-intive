using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spv.Usuarios.Common.Dtos.PersonaService.Output;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Test.Infrastructure;
using Xunit;

namespace Spv.Usuarios.Test.Integration.ExternalServices
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PersonasRepositoryIntegrationTest
    {
        private readonly IPersonasRepository _personasRepository;
        private readonly ServerFixture _server;
        public WireMockHelper WireMockHelper { get; set; }

        public PersonasRepositoryIntegrationTest(ServerFixture server)
        {
            var personasRepository = server.HttpServer.TestServer.Services.GetRequiredService<IPersonasRepository>();

            _personasRepository = personasRepository;
            _server = server;
            WireMockHelper = _server.WireMock;
        }

        [Fact]
        public async Task GetPersonaOkAsync()
        {
            // Arrange
            var expectedPerson = new PersonaModelResponse
            {
                id = 100,
                tipo_persona = "Test tipo_persona",
                links = new Links
                {
                    empty = false
                }
            };

            var path = $"{ApiPersonasUris.Persona("Exitoso", 1, 1)}";

            _server.WireMock.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(expectedPerson));

            // Act
            var result = await _personasRepository.ObtenerPersona("Exitoso", 1, 1, "channel", "user");

            // Assert
            result.Should().NotBeNull();
            result.id.Should().Be(100);
            result.tipo_persona.Should().Be("Test tipo_persona");
            result.links.empty.Should().Be(false);
        }

        [Fact]
        public async Task GetPersonaErrorAsync()
        {
            // Arrange
            var path = $"{ApiPersonasUris.Persona("No exitoso", 2, 2)}";

            _server.WireMock.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _personasRepository.ObtenerPersona("No exitoso", 2, 2, "channel", "user");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetInfoPersonaOkAsync()
        {
            // Arrange
            var expectedInfoPerson = new PersonaInfoModelResponse
            {
                id = 1,
                numero_documento = "numero_documento",
                tipo_persona = "Tipo Persona Test",
                actividades_economicas_afip = new List<ActividadEconomicaAfip>
                {
                    new ActividadEconomicaAfip
                    {
                        codigo_actividad = 1,
                        descripcion_actividad = "descripcion_actividad",
                        fecha_actualizacion = "fecha_actualizacion",
                        id = 1,
                        orden = 1
                    }
                },
                canal_creacion = "canal_creacion",
                canal_distribucion = 1,
                canal_modificacion = "canal_modificacion",
                categoria = "categoria",
                declaracion_fatca = new DeclaracionFatca
                {
                    categoria = 1,
                    numero = "numero"
                },
                declaracion_ocde = new DeclaracionOcde
                {
                    declara_ocde = true,
                    fecha_declaracion = "fecha_declaracion",
                    identificador_unico_ocde = "identificador_unico_ocde"
                },
                declaracion_uif = new DeclaracionUif
                {
                    es_sujeto_obligado_uif = true,
                    tipo_sujeto_obligado_uif = 1
                },
                documentos_adicionales = new List<DocumentoAdicional>
                {
                    new DocumentoAdicional
                    {
                        canal_creacion = "canal_creacion",
                        canal_modificacion = "canal_modificacion",
                        fecha_creacion = "fecha_creacion",
                        fecha_modificacion = "fecha_modificacion",
                        id = 1,
                        numero_documento = "numero_documento",
                        pais_documento = 1,
                        tipo_documento = 1
                    }
                },
                domicilios = new List<Domicilio>
                {
                    new Domicilio
                    {
                        id = 1,
                        calle = "calle",
                        fecha_modificacion = "fecha_modificacion",
                        canal_creacion = "canal_creacion",
                        canal_modificacion = "canal_modificacion",
                        codigo_postal = 1,
                        codigo_postal_argentino = "codigo_postal_argentino",
                        departamento = "departamento",
                        fecha_creacion = "fecha_creacion",
                        latitud = "latitud",
                        legal = "legal",
                        localidad = 1,
                        localidad_alfanumerica = "localidad_alfanumerica",
                        localidad_maestro = "localidad_maestro",
                        longitud = "longitud",
                        normalizado = true,
                        numero = "numero",
                        origen_contacto = "origen_contacto",
                        pais = 1,
                        piso = "piso",
                        provincia = 1,
                        usuario_creacion = "usuario_creacion",
                        usuario_modificacion = "usuario_modificacion"
                    }
                },
                usuario_modificacion = "usuario_modificacion",
                usuario_creacion = "usuario_creacion",
                emails = new List<Email>
                {
                    new Email
                    {
                        id = 1,
                        usuario_creacion = "usuario_creacion",
                        usuario_modificacion = "usuario_modificacion",
                        canal_creacion = "canal_creacion",
                        canal_modificacion = "canal_modificacion",
                        cargo_interlocutor = "cargo_interlocutor",
                        confiable = true,
                        dado_de_baja = true,
                        direccion = "direccion",
                        etiquetas = new List<Etiqueta>
                        {
                            new Etiqueta
                            {
                                codigo = "codigo",
                                descripcion = "descripcion"
                            }
                        },
                        fecha_creacion = "fecha_creacion",
                        fecha_modificacion = "fecha_modificacion",
                        nombre_interlocutor = "nombre_interlocutor",
                        origen_contacto = "origen_contacto",
                        principal = true
                    }
                },
                fecha_modificacion = "fecha_modificacion",
                fecha_creacion = "fecha_creacion",
                estado_validacion_documento = "estado_validacion_documento",
                fecha_alta_bt = "fecha_alta_bt",
                fecha_baja_bt = "fecha_baja_bt",
                impuestos = new List<Impuesto>
                {
                    new Impuesto
                    {
                        id = 1,
                        fecha_creacion = "fecha_creacion",
                        fecha_modificacion = "fecha_modificacion",
                        condicion = 1,
                        tipo_impuesto = 1
                    }
                },
                numero_tributario = "numero_tributario",
                pais_documento = 1,
                pais_residencia = 1,
                telefonos = new List<Telefono>
                {
                    new Telefono
                    {
                        id = 1,
                        canal_creacion = "canal_creacion",
                        canal_modificacion = "canal_modificacion",
                        cargo_interlocutor = "cargo_interlocutor",
                        codigo_area = 1,
                        compania = "compania",
                        confiable = true,
                        dado_de_baja = true,
                        ddi = "ddi",
                        ddn = "ddn",
                        doble_factor = true,
                        es_geografico = true,
                        etiquetas = new List<Etiqueta>
                        {
                            new Etiqueta
                            {
                                codigo = "codigo",
                                descripcion = "descripcion"
                            }
                        },
                        fecha_alta_no_llame = "fecha_alta_no_llame",
                        fecha_baja_no_llame = "fecha_baja_no_llame",
                        fecha_creacion = "fecha_creacion",
                        fecha_modificacion = "fecha_modificacion",
                        interno = 1,
                        nombre_interlocutor = "nombre_interlocutor",
                        normalizado = true,
                        no_llame = "no_llame",
                        numero = "numero",
                        numero_local = 1,
                        origen_contacto = "origen_contacto",
                        pais = 1,
                        prefijo_telefonico_pais = 1,
                        principal = true,
                        score = 1,
                        tipo_telefono = "tipo_telefono",
                        usuario_creacion = "usuario_creacion",
                        usuario_modificacion = "usuario_modificacion"
                    }
                },
                tipo_documento = 1,
                tipo_tributario = 1,
                tokens_push = new List<TokenPush>
                {
                    new TokenPush
                    {
                        id = 1,
                        usuario_modificacion = "usuario_modificacion",
                        usuario_creacion = "usuario_creacion",
                        canal_creacion = "canal_creacion",
                        canal_modificacion = "canal_modificacion",
                        fecha_creacion = "fecha_creacion",
                        fecha_modificacion = "fecha_modificacion",
                        sistema_operativo = "sistema_operativo",
                        token = "token"
                    }
                }
            };

            var path = $"{ApiPersonasUris.PersonaInfo(1)}";

            _server.WireMock.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(expectedInfoPerson));

            // Act
            var result = await _personasRepository.ObtenerInfoPersona(1, "OBI", "user");

            // Assert
            result.Should().NotBeNull();
            result.id.Should().Be(1);
            result.tipo_persona.Should().Be("Tipo Persona Test");
            result.numero_documento.Should().Be("numero_documento");
            result.canal_creacion.Should().Be("canal_creacion");
            result.canal_distribucion.Should().Be(1);
            result.canal_modificacion.Should().Be("canal_modificacion");
            result.categoria.Should().Be("categoria");
            result.estado_validacion_documento.Should().Be("estado_validacion_documento");
            result.fecha_alta_bt.Should().Be("fecha_alta_bt");
            result.fecha_baja_bt.Should().Be("fecha_baja_bt");
            result.fecha_creacion.Should().Be("fecha_creacion");
            result.fecha_modificacion.Should().Be("fecha_modificacion");
            result.id.Should().Be(1);
            result.numero_tributario.Should().Be("numero_tributario");
            result.pais_documento.Should().Be(1);
            result.pais_residencia.Should().Be(1);
            result.tipo_documento.Should().Be(1);
            result.tipo_tributario.Should().Be(1);
            result.usuario_creacion.Should().Be("usuario_creacion");
            result.usuario_modificacion.Should().Be("usuario_modificacion");

            result.declaracion_fatca.Should().NotBeNull();
            result.declaracion_fatca.categoria.Should().Be(1);
            result.declaracion_fatca.numero.Should().Be("numero");

            result.declaracion_ocde.Should().NotBeNull();
            result.declaracion_ocde.declara_ocde.Should().Be(true);
            result.declaracion_ocde.fecha_declaracion.Should().Be("fecha_declaracion");
            result.declaracion_ocde.identificador_unico_ocde.Should().Be("identificador_unico_ocde");

            result.declaracion_uif.Should().NotBeNull();
            result.declaracion_uif.es_sujeto_obligado_uif.Should().Be(true);
            result.declaracion_uif.tipo_sujeto_obligado_uif.Should().Be(1);

            #region emails
            result.emails.Should().NotBeNull();
            result.emails.Count.Should().Be(1);

            var email = result.emails.First();
            email.canal_creacion.Should().Be("canal_creacion");
            email.canal_modificacion.Should().Be("canal_modificacion");
            email.cargo_interlocutor.Should().Be("cargo_interlocutor");
            email.confiable.Should().Be(true);
            email.dado_de_baja.Should().Be(true);
            email.direccion.Should().Be("direccion");
            email.fecha_creacion.Should().Be("fecha_creacion");
            email.fecha_modificacion.Should().Be("fecha_modificacion");
            email.id.Should().Be(1);
            email.nombre_interlocutor.Should().Be("nombre_interlocutor");
            email.origen_contacto.Should().Be("origen_contacto");
            email.principal.Should().Be(true);
            email.usuario_creacion.Should().Be("usuario_creacion");
            email.usuario_modificacion.Should().Be("usuario_modificacion");

            email.etiquetas.Should().NotBeNull();
            email.etiquetas.Count.Should().Be(1);
            email.etiquetas.First().codigo.Should().Be("codigo");
            email.etiquetas.First().descripcion.Should().Be("descripcion");
            #endregion emails

            #region documentos_adicionales
            result.documentos_adicionales.Should().NotBeNull();
            result.documentos_adicionales.Count.Should().Be(1);

            var documentoAdicional = result.documentos_adicionales.First();
            documentoAdicional.canal_creacion.Should().Be("canal_creacion");
            documentoAdicional.canal_modificacion.Should().Be("canal_modificacion");
            documentoAdicional.fecha_creacion.Should().Be("fecha_creacion");
            documentoAdicional.fecha_modificacion.Should().Be("fecha_modificacion");
            documentoAdicional.id.Should().Be(1);
            documentoAdicional.numero_documento.Should().Be("numero_documento");
            documentoAdicional.pais_documento.Should().Be(1);
            documentoAdicional.tipo_documento.Should().Be(1);
            #endregion documentos_adicionales

            #region impuestos
            result.impuestos.Should().NotBeNull();
            result.impuestos.Count.Should().Be(1);

            var impuesto = result.impuestos.First();
            impuesto.condicion.Should().Be(1);
            impuesto.fecha_creacion.Should().Be("fecha_creacion");
            impuesto.fecha_modificacion.Should().Be("fecha_modificacion");
            impuesto.id.Should().Be(1);
            impuesto.tipo_impuesto.Should().Be(1);
            #endregion impuestos

            #region domicilios
            result.domicilios.Should().NotBeNull();
            result.domicilios.Count.Should().Be(1);

            var domicilio = result.domicilios.First();
            domicilio.calle.Should().Be("calle");
            domicilio.canal_creacion.Should().Be("canal_creacion");
            domicilio.canal_modificacion.Should().Be("canal_modificacion");
            domicilio.codigo_postal.Should().Be(1);
            domicilio.codigo_postal_argentino.Should().Be("codigo_postal_argentino");
            domicilio.departamento.Should().Be("departamento");
            domicilio.fecha_creacion.Should().Be("fecha_creacion");
            domicilio.fecha_modificacion.Should().Be("fecha_modificacion");
            domicilio.id.Should().Be(1);
            domicilio.latitud.Should().Be("latitud");
            domicilio.legal.Should().Be("legal");
            domicilio.localidad.Should().Be(1);
            domicilio.localidad_alfanumerica.Should().Be("localidad_alfanumerica");
            domicilio.localidad_maestro.Should().Be("localidad_maestro");
            domicilio.longitud.Should().Be("longitud");
            domicilio.normalizado.Should().Be(true);
            domicilio.numero.Should().Be("numero");
            domicilio.origen_contacto.Should().Be("origen_contacto");
            domicilio.pais.Should().Be(1);
            domicilio.piso.Should().Be("piso");
            domicilio.provincia.Should().Be(1);
            domicilio.usuario_creacion.Should().Be("usuario_creacion");
            domicilio.usuario_modificacion.Should().Be("usuario_modificacion");
            #endregion domicilios

            #region telefonos
            result.telefonos.Should().NotBeNull();
            result.telefonos.Count.Should().Be(1);

            var telefono = result.telefonos.First();
            telefono.canal_creacion.Should().Be("canal_creacion");
            telefono.canal_modificacion.Should().Be("canal_modificacion");
            telefono.cargo_interlocutor.Should().Be("cargo_interlocutor");
            telefono.codigo_area.Should().Be(1);
            telefono.compania.Should().Be("compania");
            telefono.confiable.Should().Be(true);
            telefono.dado_de_baja.Should().Be(true);
            telefono.ddi.Should().Be("ddi");
            telefono.ddn.Should().Be("ddn");
            telefono.doble_factor.Should().Be(true);
            telefono.es_geografico.Should().Be(true);
            telefono.fecha_alta_no_llame.Should().Be("fecha_alta_no_llame");
            telefono.fecha_baja_no_llame.Should().Be("fecha_baja_no_llame");
            telefono.fecha_creacion.Should().Be("fecha_creacion");
            telefono.fecha_modificacion.Should().Be("fecha_modificacion");
            telefono.id.Should().Be(1);
            telefono.interno.Should().Be(1);
            telefono.no_llame.Should().Be("no_llame");
            telefono.nombre_interlocutor.Should().Be("nombre_interlocutor");
            telefono.normalizado.Should().Be(true);
            telefono.numero.Should().Be("numero");
            telefono.numero_local.Should().Be(1);
            telefono.origen_contacto.Should().Be("origen_contacto");
            telefono.pais.Should().Be(1);
            telefono.prefijo_telefonico_pais.Should().Be(1);
            telefono.principal.Should().Be(true);
            telefono.score.Should().Be(1);
            telefono.tipo_telefono.Should().Be("tipo_telefono");
            telefono.usuario_creacion.Should().Be("usuario_creacion");
            telefono.usuario_modificacion.Should().Be("usuario_modificacion");

            telefono.etiquetas.Should().NotBeNull();
            telefono.etiquetas.Count.Should().Be(1);
            telefono.etiquetas.First().codigo.Should().Be("codigo");
            telefono.etiquetas.First().descripcion.Should().Be("descripcion");
            #endregion telefonos

            #region tokens_push
            result.tokens_push.Should().NotBeNull();
            result.tokens_push.Count.Should().Be(1);

            var token = result.tokens_push.First();
            token.canal_creacion.Should().Be("canal_creacion");
            token.canal_modificacion.Should().Be("canal_modificacion");
            token.fecha_creacion.Should().Be("fecha_creacion");
            token.fecha_modificacion.Should().Be("fecha_modificacion");
            token.id.Should().Be(1);
            token.sistema_operativo.Should().Be("sistema_operativo");
            token.token.Should().Be("token");
            token.usuario_creacion.Should().Be("usuario_creacion");
            token.usuario_modificacion.Should().Be("usuario_modificacion");
            #endregion tokens_push
        }

        [Fact]
        public async Task GetInfoPersonaErrorAsync()
        {
            // Arrange
            var path = $"{ApiPersonasUris.PersonaInfo(2)}";

            _server.WireMock.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.RespondWithBadRequest());

            // Act
            var result = await _personasRepository.ObtenerInfoPersona(2, "channel", "user");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetInfoPersonaFisicaOkAsync()
        {
            // Arrange
            var personaFisicaInfoModelResponse = new PersonaFisicaInfoModelResponse
            {
                id = 14155916,
                pais_documento = 80,
                tipo_documento = 4,
                numero_documento = "23734745",
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
                        dado_de_baja = false,
                        ultima_verificacion_positiva = new UltimaVerificacionPositiva()
                        {
                            canal_verificacion = "OBI",
                            fecha_verificacion = "2021-01-07 15:12:09.1049280 -03:00",
                            usuario_verificacion = "18"
                        }
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

            var path = $"{ApiPersonasUris.PersonaFisicaInfo(14155916)}";

            _server.WireMock.ServiceMock
                .Given(WireMockHelper.Get(path))
                .RespondWith(WireMockHelper.Json(personaFisicaInfoModelResponse));

            // Act
            var result = await _personasRepository.ObtenerInfoPersonaFisica(14155916, "OBI", "usuario");

            // Assert
            result.Should().NotBeNull();
            result.id.Should().Be(14155916);
            result.tipo_persona.Should().Be("F");
            result.numero_documento.Should().Be("23734745");
            result.estado_validacion_documento.Should().Be("NO_VALIDADO");
            result.fecha_creacion.Should().Be("2019-12-03 12:04:05.0311100 -03:00");
            result.fecha_modificacion.Should().Be("2020-10-07 06:44:23.8278290 -03:00");
            result.pais_documento.Should().Be(80);
            result.tipo_documento.Should().Be(4);

            result.declaracion_fatca.Should().BeNull();

            result.declaracion_ocde.Should().NotBeNull();
            result.declaracion_ocde.declara_ocde.Should().Be(false);
            result.declaracion_ocde.fecha_declaracion.Should().BeNullOrWhiteSpace();
            result.declaracion_ocde.identificador_unico_ocde.Should().BeNullOrWhiteSpace();

            result.declaracion_uif.Should().BeNull();

            #region emails
            result.emails.Should().NotBeNull();
            result.emails.Count.Should().Be(3);

            var email = result.emails.First();
            email.canal_creacion.Should().Be("HBI");
            email.canal_modificacion.Should().BeNullOrWhiteSpace();
            email.cargo_interlocutor.Should().BeNullOrWhiteSpace();
            email.confiable.Should().Be(false);
            email.dado_de_baja.Should().Be(false);
            email.direccion.Should().Be("asdfgh@simtlix.com");
            email.fecha_creacion.Should().Be("2020-11-18 17:03:45.9769930 -03:00");
            email.fecha_modificacion.Should().BeNullOrWhiteSpace();
            email.id.Should().Be(29140);
            email.nombre_interlocutor.Should().BeNullOrWhiteSpace();
            email.origen_contacto.Should().Be("PERSONAL");
            email.principal.Should().Be(false);
            email.usuario_creacion.Should().Be("test");
            email.usuario_modificacion.Should().BeNullOrWhiteSpace();

            email.etiquetas.Should().NotBeNull();
            email.etiquetas.Count.Should().Be(1);
            email.etiquetas.First().codigo.Should().Be("NOTIFICACIONES_MODO");
            email.etiquetas.First().descripcion.Should().Be("Envío de Notificaciones por MODO");
            #endregion emails

            #region tokens_push
            result.tokens_push.Should().NotBeNull();
            result.tokens_push.Count.Should().Be(1);

            var token = result.tokens_push.First();
            token.canal_creacion.Should().Be("MBI");
            token.canal_modificacion.Should().BeNullOrWhiteSpace();
            token.fecha_creacion.Should().Be("2021-02-11 15:14:59.7553090 -03:00");
            token.fecha_modificacion.Should().BeNullOrWhiteSpace();
            token.id.Should().Be(22687);
            token.sistema_operativo.Should().Be("IOS");
            token.token.Should().Be("token_nuevo_de_la_persona_14155917");
            token.usuario_creacion.Should().BeNullOrWhiteSpace();
            token.usuario_modificacion.Should().BeNullOrWhiteSpace();
            #endregion tokens_push

            #region telefonos
            result.telefonos.Should().NotBeNull();
            result.telefonos.Count.Should().Be(3);

            var telefono = result.telefonos.First();
            telefono.canal_creacion.Should().Be("HBI");
            telefono.canal_modificacion.Should().BeNullOrWhiteSpace();
            telefono.cargo_interlocutor.Should().BeNullOrWhiteSpace();
            telefono.codigo_area.Should().Be(11);
            telefono.compania.Should().Be("CLARO");
            telefono.confiable.Should().Be(true);
            telefono.dado_de_baja.Should().Be(false);
            telefono.ddi.Should().Be("+54 9 11 2345 4356");
            telefono.ddn.Should().Be("0111523454356");
            telefono.doble_factor.Should().BeNull();
            telefono.es_geografico.Should().Be(true);
            telefono.fecha_alta_no_llame.Should().BeNullOrWhiteSpace();
            telefono.fecha_baja_no_llame.Should().BeNullOrWhiteSpace();
            telefono.fecha_creacion.Should().Be("2021-01-07 15:12:09.1049280 -03:00");
            telefono.fecha_modificacion.Should().BeNullOrWhiteSpace();
            telefono.id.Should().Be(762855);
            telefono.interno.Should().BeNull();
            telefono.no_llame.Should().Be("false");
            telefono.nombre_interlocutor.Should().BeNullOrWhiteSpace();
            telefono.normalizado.Should().Be(true);
            telefono.numero.Should().Be("1123454356");
            telefono.numero_local.Should().Be(23454356);
            telefono.origen_contacto.Should().Be("PERSONAL");
            telefono.pais.Should().Be(80);
            telefono.prefijo_telefonico_pais.Should().Be(54);
            telefono.principal.Should().BeNull();
            telefono.score.Should().BeNull();
            telefono.tipo_telefono.Should().Be("CELULAR");
            telefono.usuario_creacion.Should().Be("18");
            telefono.usuario_modificacion.Should().BeNullOrWhiteSpace();

            telefono.etiquetas.Should().NotBeNull();
            telefono.etiquetas.Count.Should().Be(0);
            #endregion telefonos

            #region domicilios
            result.domicilios.Should().NotBeNull();
            result.domicilios.Count.Should().Be(1);

            var domicilio = result.domicilios.First();
            domicilio.calle.Should().Be("CHARCAS");
            domicilio.canal_creacion.Should().Be("BANTOTAL");
            domicilio.canal_modificacion.Should().Be("BANTOTAL");
            domicilio.codigo_postal.Should().Be(5521);
            domicilio.codigo_postal_argentino.Should().Be("M5521NVF");
            domicilio.departamento.Should().Be("");
            domicilio.fecha_creacion.Should().Be("2019-12-03 12:04:05.0318220 -03:00");
            domicilio.fecha_modificacion.Should().Be("2020-10-07 06:44:23.8425130 -03:00");
            domicilio.id.Should().Be(141);
            domicilio.latitud.Should().BeNullOrWhiteSpace();
            domicilio.legal.Should().Be("true");
            domicilio.localidad.Should().Be(548);
            domicilio.localidad_alfanumerica.Should().BeNullOrWhiteSpace();
            domicilio.localidad_maestro.Should().Be("VILLA NUEVA DE GUAYMALLEN");
            domicilio.longitud.Should().BeNullOrWhiteSpace();
            domicilio.normalizado.Should().Be(true);
            domicilio.numero.Should().Be("4116");
            domicilio.origen_contacto.Should().BeNullOrWhiteSpace();
            domicilio.pais.Should().Be(80);
            domicilio.piso.Should().Be("");
            domicilio.provincia.Should().Be(13);
            domicilio.usuario_creacion.Should().BeNullOrWhiteSpace();
            domicilio.usuario_modificacion.Should().BeNullOrWhiteSpace();
            #endregion domicilios

            #region documentos_adicionales
            result.documentos_adicionales.Should().NotBeNull();
            result.documentos_adicionales.Count.Should().Be(0);
            #endregion documentos_adicionales

            #region actividades_economicas_afip
            result.actividades_economicas_afip.Should().NotBeNull();
            result.actividades_economicas_afip.Count.Should().Be(0);
            #endregion actividades_economicas_afip

            #region impuestos
            result.impuestos.Should().NotBeNull();
            result.impuestos.Count.Should().Be(7);

            var impuesto = result.impuestos.First();
            impuesto.condicion.Should().Be(1);
            impuesto.fecha_creacion.Should().Be("2019-12-03 12:04:04.8746150 -03:00");
            impuesto.fecha_modificacion.Should().BeNullOrWhiteSpace();
            impuesto.id.Should().Be(212);
            impuesto.tipo_impuesto.Should().Be(1);
            #endregion impuestos

            result.tipo_persona.Should().Be("F");
            result.fecha_alta_bt.Should().Be("1998-10-26");
            result.fecha_baja_bt.Should().BeNullOrWhiteSpace();
            result.categoria.Should().Be("N");
            result.canal_distribucion.Should().Be(2);
            result.canal_creacion.Should().Be("BANTOTAL");
            result.usuario_creacion.Should().BeNullOrWhiteSpace();
            result.canal_modificacion.Should().Be("BANTOTAL");
            result.usuario_modificacion.Should().BeNullOrWhiteSpace();
            result.tipo_tributario.Should().Be(2);
            result.numero_tributario.Should().Be("20237347456");

            result.declaracion_ocde.Should().NotBeNull();
            result.declaracion_ocde.declara_ocde.Should().BeFalse();
            result.declaracion_ocde.fecha_declaracion.Should().BeNullOrWhiteSpace();
            result.declaracion_ocde.identificador_unico_ocde.Should().BeNullOrWhiteSpace();

            result.declaracion_fatca.Should().BeNull();
            result.declaracion_uif.Should().BeNull();
            result.estado_validacion_documento.Should().Be("NO_VALIDADO");
            result.pais_residencia.Should().Be(10);
            result.nombre.Should().Be("RICARDO CRISTIAN");
            result.apellido.Should().Be("BERTOLDO");
            result.genero.Should().Be("M");
            result.fecha_nacimiento.Should().Be("1974-02-10");
            result.estado_civil.Should().Be(2);
            result.paquete.Should().BeNull();
            result.pais_nacimiento.Should().Be(80);
            result.es_ciudadano_legal.Should().BeTrue();
            result.marca_acredita_sueldo.Should().BeNull();
            result.es_empleado.Should().BeFalse();
            result.registro_patrimonial_matrimonio.Should().BeNull();
            result.lugar_nacimiento.Should().Be("ARGENTINA (REPU");

            #region vinculos_de_personas
            result.vinculos_de_personas.Count.Should().Be(1);

            var vinculoDePersona = result.vinculos_de_personas.First();
            vinculoDePersona.canal_creacion.Should().Be("string");
            vinculoDePersona.fecha_creacion.Should().Be("string");
            vinculoDePersona.id.Should().Be(0);
            vinculoDePersona.id_persona_fisica.Should().Be(0);
            vinculoDePersona.id_persona_fisica_vinculada.Should().Be(0);
            vinculoDePersona.usuario_creacion.Should().Be("string");
            vinculoDePersona.vinculo.Should().Be(0);
            #endregion vinculos_de_personas

            result.estado_persona_expuesta_politicamente.Should().NotBeNull();
            result.estado_persona_expuesta_politicamente.esta_expuesta.Should().BeFalse();
            result.estado_persona_expuesta_politicamente.motivo.Should().BeNullOrWhiteSpace();
            result.fallecido.Should().BeTrue();
            result.fecha_de_fallecimiento.Should().BeNullOrWhiteSpace();
            result.fecha_de_consulta_renaper.Should().BeNullOrWhiteSpace();
            result.fecha_creacion.Should().Be("2019-12-03 12:04:05.0311100 -03:00");
            result.fecha_modificacion.Should().Be("2020-10-07 06:44:23.8278290 -03:00");
            result.es_titular.Should().BeTrue();
        }
    }
}
