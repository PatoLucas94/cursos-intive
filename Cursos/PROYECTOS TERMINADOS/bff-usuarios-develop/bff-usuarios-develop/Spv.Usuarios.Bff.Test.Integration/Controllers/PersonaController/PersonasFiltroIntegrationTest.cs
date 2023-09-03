using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.CatalogoClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Errors;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.PersonaController.CommonPersona.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.PersonaController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class PersonasFiltroIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        // Persona Correcta
        private const int IdPaisPersonaCorrecta = 80;
        private const int IdTipoDocumentoPersonaCorrecta = 4;
        private const string NroDocumentoPersonaCorrecta = "23734745";
        private const int IdPersonaCorrecta = 14155917;
        private const string NroDocumentoDuplicadoLeLc = "1011781";
        private const int IdPersonaDuplicadoLe = 16804753;
        private const int IdPersonaDuplicadoLc = 16484507;
        private const string NroDocumentoDuplicadoPasaporte = "3949";
        private const int IdPersonaPasaporte1 = 15520026;
        private const int IdPersonaPasaporte2 = 16705747;

        // Persona Fisica Inexistente
        private static readonly string nroDocumentoPersonaFisicaInexistente = "11222333";
        private static readonly int idPersonaFisicaInexistente = 1;

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    NroDocumentoPersonaCorrecta, IdPersonaCorrecta, HttpStatusCode.OK
                },
                new object[]
                {
                    "11111111", 1, HttpStatusCode.NotFound
                },
                new object[]
                {
                    nroDocumentoPersonaFisicaInexistente, idPersonaFisicaInexistente, HttpStatusCode.InternalServerError
                },
                new object[]
                {
                    NroDocumentoDuplicadoLeLc, null, HttpStatusCode.OK
                },
                new object[]
                {
                    NroDocumentoDuplicadoPasaporte, null, HttpStatusCode.OK
                }
            };

        private static IEnumerable<ApiPersonasFiltroModelOutput> ApiPersonasFiltroModelOutput =>
            new List<ApiPersonasFiltroModelOutput>
            {
                new ApiPersonasFiltroModelOutput
                {
                    id = IdPersonaCorrecta,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = NroDocumentoPersonaCorrecta,
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
                        },
                        new EmailExtraInfo
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
                        new EmailExtraInfo
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
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new Impuesto
                        {
                            id = 212,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 213,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 214,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 215,
                            tipo_impuesto = 5,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 216,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 217,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
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
                    fecha_creacion = "2019-12-03 12:04:05.0311100 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8278290 -03:00",
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaFisicaInexistente,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = NroDocumentoPersonaCorrecta,
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
                        },
                        new EmailExtraInfo
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
                        new EmailExtraInfo
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
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new Impuesto
                        {
                            id = 212,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 213,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 214,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 215,
                            tipo_impuesto = 5,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 216,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 217,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
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
                    fecha_creacion = "2019-12-03 12:04:05.0311100 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8278290 -03:00",
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = 852,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = NroDocumentoPersonaCorrecta,
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
                        },
                        new EmailExtraInfo
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
                        new EmailExtraInfo
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
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new TelefonoExtraInfo
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
                        new Impuesto
                        {
                            id = 212,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 213,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 214,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 215,
                            tipo_impuesto = 5,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 216,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 217,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
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
                    fecha_creacion = "2019-12-03 12:04:05.0311100 -03:00",
                    fecha_modificacion = "2020-10-07 06:44:23.8278290 -03:00",
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = IdPersonaDuplicadoLc,
                    pais_documento = 80,
                    tipo_documento = (int)TipoDocumento.Lc,
                    numero_documento = NroDocumentoDuplicadoLeLc,
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2583052,
                            fecha_creacion = "2021-08-20 18:15:53.0791280 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "MARTINEZ DE ROSAS",
                            numero = "2612",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "K4700XAK",
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
                        new Impuesto
                        {
                            id = 16208723,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006510 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208724,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-20 18:15:53.1006610 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208725,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006670 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208726,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-20 18:15:53.1006730 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208727,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006780 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208729,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007260 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208731,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007370 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208733,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208735,
                            tipo_impuesto = 50,
                            condicion = 5,
                            fecha_creacion = "2021-08-20 18:15:53.1007520 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-04-12",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "27010117815",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_enriquecimiento_afip = null,
                    fecha_enriquecimiento = null,
                    fecha_creacion = "2021-08-20 18:15:53.0788400 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = IdPersonaDuplicadoLe,
                    pais_documento = 80,
                    tipo_documento = (int)TipoDocumento.Le,
                    numero_documento = NroDocumentoDuplicadoLeLc,
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2918440,
                            fecha_creacion = "2021-08-21 11:54:31.4979910 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "MARTINEZ DE ROSAS",
                            numero = "2612",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "K4700XAK",
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
                        new Impuesto
                        {
                            id = 19875822,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107720 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875823,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 11:54:31.5107820 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875824,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875825,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 11:54:31.5107930 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875826,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107990 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875827,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5108050 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875828,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5108100 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875829,

                            tipo_impuesto = 10,

                            condicion = 1,

                            fecha_creacion = "2021-08-21 11:54:31.5108160 -03:00",

                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875830,
                            tipo_impuesto = 50,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 11:54:31.5108210 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-04-12",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "24010117816",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_enriquecimiento_afip = null,
                    fecha_enriquecimiento = null,
                    fecha_creacion = "2021-08-21 11:54:31.4978590 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = 15520026,
                    pais_documento = 129,
                    tipo_documento = 10,
                    numero_documento = "3949",
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 1556724,
                            fecha_creacion = "2021-07-09 11:39:38.5519630 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "25 DE MAYO",
                            numero = "64",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "M5500EUB",
                            latitud = "-32.9023075",
                            longitud = "-68.8501663",
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true"
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 8087332,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5633510 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087333,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5633640 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087334,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5633740 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087335,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2021-07-09 11:39:38.5633850 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087336,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5633930 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087337,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5634020 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 8087338,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2021-07-09 11:39:38.5634090 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2009-09-15",
                    fecha_baja_bt = null,
                    categoria = "S",
                    canal_distribucion = null,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "JH75618",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "27625308947",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_enriquecimiento_afip = null,
                    fecha_enriquecimiento = null,
                    fecha_creacion = "2021-07-09 11:39:38.5517700 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = 16705747,
                    pais_documento = 4,
                    tipo_documento = 10,
                    numero_documento = "3949",
                    emails = new List<EmailExtraInfo>
                    {
                        new EmailExtraInfo
                        {
                            id = 429922,
                            direccion = "nomail@optimspv.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2021-08-21 07:13:10.7745020 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            etiquetas =  new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            dado_de_baja = false
                        }
                    },
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2814585,
                            fecha_creacion = "2021-08-21 07:13:10.7521920 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "25 DE MAYO",
                            numero = "64",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "C1091AAQ",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true"
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 18775453,
                            tipo_impuesto = 1,
                            condicion = 3,
                            fecha_creacion = "2021-08-21 07:13:10.7761030 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775454,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 07:13:10.7761120 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775455,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:10.7761180 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775456,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 07:13:10.7761280 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775457,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:10.7761330 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775458,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:10.7761380 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775459,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:10.7761440 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775460,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:10.7761500 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18775461,
                            tipo_impuesto = 50,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 07:13:10.7761550 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2010-10-15",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "20608348302",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_enriquecimiento_afip = null,
                    fecha_enriquecimiento = null,
                    fecha_creacion = "2021-08-21 07:13:10.7519980 -03:00",
                    fecha_modificacion = null
                }
            };

        private static IEnumerable<ApiPersonasFisicaInfoModelOutput> ApiPersonaFisicaInfoOutput =>
            new List<ApiPersonasFisicaInfoModelOutput>
            {
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = IdPersonaCorrecta,
                    pais_documento = IdPaisPersonaCorrecta,
                    tipo_documento = IdTipoDocumentoPersonaCorrecta,
                    numero_documento = NroDocumentoPersonaCorrecta,
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
                        new Impuesto
                        {
                            id = 212,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 213,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 214,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 215,
                            tipo_impuesto = 5,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 216,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 217,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
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
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = 852,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "11222333",
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
                        },
                        new Email
                        {
                            id = 28934,
                            direccion = "test@test.com",
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
                        new Impuesto
                        {
                            id = 212,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8746150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 213,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8747140 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 214,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8748430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 215,
                            tipo_impuesto = 5,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8758110 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 216,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8762870 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 217,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2019-12-03 12:04:04.8767580 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
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
                    nombre = "Usuario",
                    apellido = "Test1",
                    genero = "F",
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
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = IdPersonaDuplicadoLe,
                    pais_documento = 80,
                    tipo_documento = (int)TipoDocumento.Le,
                    numero_documento = NroDocumentoDuplicadoLeLc,
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2918440,
                            fecha_creacion = "2021-08-21 11:54:31.4979910 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "MARTINEZ DE ROSAS",
                            numero = "2612",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "K4700XAK",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true"
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 19875822,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107720 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875823,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 11:54:31.5107820 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875824,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875825,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 11:54:31.5107930 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875826,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5107990 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875827,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5108050 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875828,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5108100 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875829,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 11:54:31.5108160 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 19875830,
                            tipo_impuesto = 50,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 11:54:31.5108210 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-04-12",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "24010117816",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_creacion = "2021-08-21 11:54:31.4978590 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = IdPersonaDuplicadoLc,
                    pais_documento = 80,
                    tipo_documento = (int)TipoDocumento.Lc,
                    numero_documento = NroDocumentoDuplicadoLeLc,
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2583052,
                            fecha_creacion = "2021-08-20 18:15:53.0791280 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 13,
                            localidad = 403,
                            localidad_maestro = "MENDOZA",
                            calle = "MARTINEZ DE ROSAS",
                            numero = "2612",
                            piso = "",
                            departamento = "",
                            codigo_postal = 5500,
                            codigo_postal_argentino = "K4700XAK",
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
                        new Impuesto
                        {
                            id = 16208723,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006510 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208724,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-20 18:15:53.1006610 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208725,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006670 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208726,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-20 18:15:53.1006730 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208727,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1006780 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208729,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007260 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208731,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007370 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208733,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2021-08-20 18:15:53.1007430 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 16208735,
                            tipo_impuesto = 50,
                            condicion = 5,
                            fecha_creacion = "2021-08-20 18:15:53.1007520 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-04-12",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "27010117815",
                    declaracion_ocde = new DeclaracionOcde
                    {
                        declara_ocde = false,
                        fecha_declaracion = null,
                        identificador_unico_ocde = null
                    },
                    declaracion_fatca = null,
                    declaracion_uif = null,
                    estado_validacion_documento = "NO_VALIDADO",
                    pais_residencia = null,
                    fecha_creacion = "2021-08-20 18:15:53.0788400 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = IdPersonaPasaporte1,
                    pais_documento = 129,
                    tipo_documento = (int)TipoDocumento.Pasaporte,
                    numero_documento = NroDocumentoDuplicadoPasaporte,
                    emails = new List<Email>
                    {
                        new Email
                        {
                            confiable = true,
                            direccion = "confiablePasaporte1@test.com"
                        }
                    },
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            confiable = true,
                            numero = "1345237"
                        }
                    }
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = IdPersonaPasaporte2,
                    pais_documento = 4,
                    tipo_documento = (int)TipoDocumento.Pasaporte,
                    numero_documento = NroDocumentoDuplicadoPasaporte,
                    emails = new List<Email>
                    {
                        new Email
                        {
                            confiable = true,
                            direccion = "confiablePasaporte2@test.com"
                        }
                    },
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            confiable = true,
                            numero = "1345238"
                        }
                    }
                }
            };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private WireMockHelper WireMockHelper { get; set; }

        public PersonasFiltroIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        private static ServiceRequest GetPersona(
            Uri uriBase, 
            string nroDocumento,
            int? tipoDocumento = null,
            int? paisId = null)
        {
            var uri = new Uri(uriBase, ApiUris.PersonasFiltro(nroDocumento, tipoDocumento, paisId));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task PersonasFiltroAsync(string nroDocumento, int? idPersona, HttpStatusCode httpStatusCode)
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(NroDocumentoPersonaCorrecta)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaCorrecta)}";
            var pathPersonaFisicaInexistente =
                $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaFisicaInexistente)}";
            var pathPersonaNroDuplicadoLeLc = $"{ApiPersonasUris.PersonasFiltro(NroDocumentoDuplicadoLeLc)}";
            var pathPersonaFisicaDuplicadoLe = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaDuplicadoLe)}";
            var pathPersonaFisicaDuplicadoLc = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaDuplicadoLc)}";
            var pathPersonaNroDuplicadoPasaporte = $"{ApiPersonasUris.PersonasFiltro(NroDocumentoDuplicadoPasaporte)}";
            var pathPersonaFisicaPasaporte1 = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaPasaporte1)}";
            var pathPersonaFisicaPasaporte2 = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaPasaporte2)}";
            var pathValidacionExistenciaInexistente = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathCatalogoPaises = $"{ApiCatalogoUris.Paises()}";

            var apiUsuariosErrorResponse = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            var paises = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput { codigo = 4, descripcion = "FRANCIA REPUBLICA" },
                new ApiCatalogoPaisesModelOutput { codigo = 129, descripcion = "MONACO, PRINCIPADO DE" }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathCatalogoPaises))
                .WithTitle("PersonasFiltroAsync-CatalogoPaises")
                .RespondWith(WireMockHelper.Json(paises));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("PersonasFiltroAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == IdPersonaCorrecta)
                    }
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaInexistente))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaInexistente")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaFisicaInexistente)
                    }
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDuplicadoLeLc))
                .WithTitle("PersonasFiltroAsync-PersonaNroDuplicadoLeLc")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x => x.numero_documento == NroDocumentoDuplicadoLeLc)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadoLc))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaDuplicadoLc")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaDuplicadoLc)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadoLe))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaDuplicadoLe")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaDuplicadoLe)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDuplicadoPasaporte))
                .WithTitle("PersonasFiltroAsync-PersonaNroDuplicadoPasaporte")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x => x.numero_documento == NroDocumentoDuplicadoPasaporte)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPasaporte1))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaPasaporte1")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaPasaporte1)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPasaporte2))
                .WithTitle("PersonasFiltroAsync-PersonaFisicaPasaporte2")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaPasaporte2)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistenciaInexistente))
                .WithTitle("PersonasFiltroAsync-ValidacionExistenciaInexistente")
                .RespondWith(WireMockHelper.RespondWithNotFound(apiUsuariosErrorResponse));

            var request = GetPersona(_uriBase, nroDocumento);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            if (httpStatusCode == HttpStatusCode.OK)
            {
                var response = await sut.Content.ReadAsAsync<PersonaModelResponse>();

                switch (idPersona)
                {
                    case IdPersonaCorrecta:
                        response.Id.Should().Be(IdPersonaCorrecta);
                        response.PaisDocumento.Should().Be(80);
                        response.TipoDocumento.Should().Be(4);
                        response.NumeroDocumento.Should().Be(NroDocumentoPersonaCorrecta);
                        response.TipoPersona.Should().Be("F");
                        response.Nombre.Should().Be("RICARDO CRISTIAN");
                        response.Apellido.Should().Be("BERTOLDO");
                        response.Genero.Should().Be("M");
                        response.FechaNacimiento.Should().Be("1974-02-10");
                        response.PaisNacimiento.Should().Be(80);
                        response.Conflicto.Should().BeFalse();
                        break;
                    default:
                        response.Conflicto.Should().BeTrue();
                        if (nroDocumento == NroDocumentoDuplicadoLeLc)
                        {
                            response.TiposDocumento.Count.Should().BeGreaterOrEqualTo(2);
                            response.TiposDocumento.Any(x =>
                                    x.Codigo == (int)TipoDocumento.Le &&
                                    x.Descripcion == AppConstants.LibretaEnrolamiento)
                                .Should().BeTrue();
                            response.TiposDocumento.Any(x =>
                                    x.Codigo == (int)TipoDocumento.Lc &&
                                    x.Descripcion == AppConstants.LibretaCivica)
                                .Should().BeTrue();
                            response.Paises.Count.Should().Be(0);
                        }
                        else
                        {
                            response.Paises.Count.Should().BeGreaterOrEqualTo(2);
                            response.Paises.Any(x =>
                                    x.Codigo == 129 &&
                                    x.Descripcion == "MONACO, PRINCIPADO DE")
                                .Should().BeTrue();
                            response.Paises.Any(x =>
                                    x.Codigo == 4 &&
                                    x.Descripcion == "FRANCIA REPUBLICA")
                                .Should().BeTrue();
                            response.TiposDocumento.Count.Should().Be(0);
                        }

                        break;
                }
            }

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task PersonasFiltroConTipoDocumentoOkAsync()
        {
            // Arrange
            var pathPersonaNroDuplicadoLeLc = $"{ApiPersonasUris.PersonasFiltro(NroDocumentoDuplicadoLeLc)}";
            var pathPersonaFisicaDuplicadoLe = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaDuplicadoLe)}";
            var pathValidacionExistenciaInexistente = $"{ApiUsuariosUris.ValidarExistencia()}";

            var apiUsuariosErrorResponse = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDuplicadoLeLc))
                .WithTitle("PersonasFiltroConTipoDocumentoOkAsync-PersonaNroDuplicadoLeLc")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x => x.numero_documento == NroDocumentoDuplicadoLeLc)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadoLe))
                .WithTitle("PersonasFiltroConTipoDocumentoOkAsync-PersonaFisicaDuplicadoLe")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaDuplicadoLe)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistenciaInexistente))
                .WithTitle("PersonasFiltroConTipoDocumentoOkAsync-ValidacionExistenciaInexistente")
                .RespondWith(WireMockHelper.RespondWithNotFound(apiUsuariosErrorResponse));

            var request = GetPersona(_uriBase, NroDocumentoDuplicadoLeLc, (int)TipoDocumento.Le);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await sut.Content.ReadAsAsync<PersonaModelResponse>();
            response.Conflicto.Should().BeFalse();
            response.Id.Should().Be(IdPersonaDuplicadoLe);
            response.NumeroDocumento.Should().Be(NroDocumentoDuplicadoLeLc);
            response.TipoDocumento.Should().Be((int)TipoDocumento.Le);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task PersonasFiltroConIdPaisOkAsync()
        {
            // Arrange
            var pathPersonaNroDuplicadoPasaporte = $"{ApiPersonasUris.PersonasFiltro(NroDocumentoDuplicadoPasaporte)}";
            var pathPersonaFisicaPasaporte2 = $"{ApiPersonasUris.PersonasFisicaInfo(IdPersonaPasaporte2)}";
            var pathValidacionExistenciaInexistente = $"{ApiUsuariosUris.ValidarExistencia()}";

            var apiUsuariosErrorResponse = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDuplicadoPasaporte))
                .WithTitle("PersonasFiltroConIdPaisOkAsync-PersonaNroDuplicadoPasaporte")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x => x.numero_documento == NroDocumentoDuplicadoPasaporte)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPasaporte2))
                .WithTitle("PersonasFiltroConIdPaisOkAsync-PersonaFisicaPasaporte2")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonaFisicaInfoOutput.First(x => x.id == IdPersonaPasaporte2)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistenciaInexistente))
                .WithTitle("PersonasFiltroConIdPaisOkAsync-ValidacionExistenciaInexistente")
                .RespondWith(WireMockHelper.RespondWithNotFound(apiUsuariosErrorResponse));

            var request = GetPersona(_uriBase, NroDocumentoDuplicadoPasaporte, null, 4);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await sut.Content.ReadAsAsync<PersonaModelResponse>();
            response.Conflicto.Should().BeFalse();
            response.Id.Should().Be(IdPersonaPasaporte2);
            response.NumeroDocumento.Should().Be(NroDocumentoDuplicadoPasaporte);
            response.TipoDocumento.Should().Be((int)TipoDocumento.Pasaporte);
            response.PaisDocumento.Should().Be(4);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
