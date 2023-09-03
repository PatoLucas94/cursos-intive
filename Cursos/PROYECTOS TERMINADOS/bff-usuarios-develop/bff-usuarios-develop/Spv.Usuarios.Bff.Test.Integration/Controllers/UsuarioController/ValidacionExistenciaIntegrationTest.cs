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
using Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Enums;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.UsuarioController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidacionExistenciaIntegrationTest : ControllerIntegrationTest
    {
        private readonly Uri _uriBase;

        // Persona Correcta
        private static readonly int idPaisPersonaCorrecta = 80;
        private static readonly int idTipoDocumentoPersonaCorrecta = (int)TipoDocumento.Dni;
        private static readonly int idTipoDocumentoDuplicado11 = (int)TipoDocumento.Le;
        private static readonly int IdTipoDocumentoDuplicado12 = (int)TipoDocumento.Lc;
        private static readonly string nroDocumentoPersonaCorrecta = "23734745";
        private static readonly string nroDocumentoPersonaCorrecta2 = "12345678";
        private static readonly string nroDocumentoDuplicadoConflictoTipoDoc = "4078985";
        private static readonly string nroDocumentoDuplicadoConflictoPais = "31187";
        private static readonly string nroDocumentoDuplicadoConflictoDniVsLibreta = "8274996";
        private static readonly int idPersonaCorrecta = 14155917;
        private static readonly int idPersonaCorrecta2 = 11112222;
        private static readonly int idPersonaDuplicada11 = 15202077;
        private static readonly int idPersonaDuplicada12 = 15791408;
        private static readonly int idPersonaDuplicadaPasaporte11 = 14200302;
        private static readonly int idPersonaDuplicadaPasaporte12 = 16705817;
        private static readonly int idPersonaDuplicadaDniVsLibreta11 = 14457020;
        private static readonly int idPersonaDuplicadaDniVsLibreta12 = 14710788;

        // Persona Fisica Inexistente
        private static readonly string nroDocumentoPersonaFisicaInexistente = "11222333";
        private static readonly int idPersonaFisicaInexistente = 1;

        public static IEnumerable<object[]> Datos =>
            new List<object[]>
            {
                new object[]
                {
                    nroDocumentoPersonaCorrecta,
                    true,
                    "rcbertoldo1",
                    HttpStatusCode.OK,
                    HttpStatusCode.OK
                },
                new object[]
                {
                    nroDocumentoPersonaCorrecta,
                    false,
                    "rcbertoldo1",
                    HttpStatusCode.OK,
                    HttpStatusCode.OK
                },
                new object[]
                {
                    "11111111",
                    false,
                    "rcbertoldo1",
                    HttpStatusCode.NotFound,
                    HttpStatusCode.NotFound
                },
                new object[]
                {
                    nroDocumentoPersonaFisicaInexistente,
                    false,
                    "rcbertoldo1",
                    HttpStatusCode.InternalServerError,
                    HttpStatusCode.NotFound
                },
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
                    id = idPersonaCorrecta2,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = "12345678",
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
                            principal = true,
                            confiable = true,
                            doble_factor = true,
                            etiquetas = new List<Etiqueta>(),
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
                    id = idPersonaDuplicada11,
                    pais_documento = 80,
                    tipo_documento = 5,
                    numero_documento = "4078985",
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>
                    {
                        new TokenPush
                        {
                            id = 32242,
                            token =
                                "czB8B0y0054:APA91bE0XjG0ECOpTaT0Hxk2nlMiDrtn11X9YVozRGV5LAYmfDYIdgqgo5IKfgPQ8dN-k2v1sQT4upFSzKnvwbHDuBIzTTehtXZ1CX6cor9fidg3rspsIyUr8b32ofSdBay-he0DIOE0",
                            sistema_operativo = "ANDROID",
                            fecha_creacion = "2021-03-20 03:02:32.9841320 -03:00",
                            fecha_modificacion = "2021-08-21 13:22:05.7535840 -03:00",
                            canal_creacion = "APPJUB",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "RP16957"
                        }
                    },
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
                        {
                            id = 2077137,
                            numero_local = 50069999,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1150069999",
                            interno = null,
                            ddn = "0111550069999",
                            ddi = "+54 9 11 5006 9999",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2021-08-21 13:22:05.8609700 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = null,
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "CELULAR",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 1270615,
                            fecha_creacion = "2020-12-22 20:58:31.0925140 -03:00",
                            fecha_modificacion = "2021-08-21 13:22:05.7542630 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "MW93221",
                            usuario_modificacion = "RP16957",
                            pais = 80,
                            provincia = 1,
                            localidad = 1001,
                            localidad_maestro = "CABA CP1001 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV INDEPENDENCIA",
                            numero = "1940",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1001,
                            codigo_postal_argentino = "C1225AAO",
                            latitud = "-34.6183548",
                            longitud = "-58.3938446",
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>
                    {
                        new ActividadEconomicaAfip
                        {
                            id = 638503,
                            codigo_actividad = 7,
                            descripcion_actividad = "JUBILADO",
                            orden = null,
                            fecha_actualizacion = "2021-08-21 13:22:05.7897580 -03:00"
                        }
                    },
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 6360825,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924630 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 20219784,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 13:22:05.9025400 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360827,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924850 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360828,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-12-22 20:58:31.0924900 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 20219785,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 13:22:05.9025550 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360829,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924940 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360830,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924990 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2009-12-10",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "MW93221",
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "RP16957",
                    tipo_tributario = 2,
                    numero_tributario = "20040789856",
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
                    fecha_creacion = "2020-12-22 20:58:31.0925130 -03:00",
                    fecha_modificacion = "2021-08-21 13:22:05.7517040 -03:00"
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaDuplicada12,
                    pais_documento = 80,
                    tipo_documento = 6,
                    numero_documento = "4078985",
                    emails = new List<EmailExtraInfo>
                    {
                        new EmailExtraInfo
                        {
                            id = 157618,
                            direccion = "nomail@optimspv.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2021-07-10 08:32:44.2738870 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6575920 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            dado_de_baja = false
                        }
                    },
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
                        {
                            id = 1301575,
                            numero_local = 429999,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 2352,
                            numero = "2352429999",
                            interno = null,
                            ddn = "02352429999",
                            ddi = "+54 2352 42 9999",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2021-07-10 08:32:44.2542300 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6563050 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 1828962,
                            fecha_creacion = "2021-07-10 08:32:44.2252260 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6587130 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            pais = 80,
                            provincia = 1,
                            localidad = 1001,
                            localidad_maestro = "CABA CP1001 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV INDEPENDENCIA",
                            numero = "1940",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1001,
                            codigo_postal_argentino = "C1225AAO",
                            latitud = "-34.6183548",
                            longitud = "-58.3938446",
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>
                    {
                        new ActividadEconomicaAfip
                        {
                            id = 182795,
                            codigo_actividad = 471190,
                            descripcion_actividad =
                                "VENTA AL POR MENOR EN KIOSCOS, POLIRRUBROS Y COMERCIOS NO ESPECIALIZADOS N.C.P.",
                            orden = null,
                            fecha_actualizacion = "2021-08-22 15:27:50.6910480 -03:00"
                        }
                    },
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 9794009,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765560 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008043,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-22 15:27:50.7306880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794011,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765780 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008044,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-22 15:27:50.7306980 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008045,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-22 15:27:50.7307040 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794013,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765960 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794014,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2766050 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2007-08-15",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "JH75618",
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "RP16957",
                    tipo_tributario = 2,
                    numero_tributario = "27040789850",
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
                    fecha_creacion = "2021-07-10 08:32:44.2250420 -03:00",
                    fecha_modificacion = "2021-08-22 15:27:50.6474780 -03:00"
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaDuplicadaPasaporte11,
                    pais_documento = 4,
                    tipo_documento = 10,
                    numero_documento = "31187",
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
                        {
                            id = 70314,
                            numero_local = 43220068,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1143220068",
                            interno = null,
                            ddn = "01143220068",
                            ddi = "+54 11 4322 0068",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-03-25 07:35:59.8534020 -03:00",
                            fecha_modificacion = "2021-04-30 11:56:19.5592780 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "UPD_SINCRO",
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 47350,
                            fecha_creacion = "2020-03-25 07:35:59.8534050 -03:00",
                            fecha_modificacion = "2021-04-30 11:56:19.5602860 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "UPD_SINCRO",
                            pais = 80,
                            provincia = 1,
                            localidad = 1054,
                            localidad_maestro = "CABA CP1054 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV CORDOBA",
                            numero = "946",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1054,
                            codigo_postal_argentino = "C1054AAV",
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
                            id = 171509,
                            tipo_impuesto = 14,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510330 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171508,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510180 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171507,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510030 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171506,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8509880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171505,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-03-25 07:35:59.8509730 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171504,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8509590 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171503,
                            tipo_impuesto = 2,
                            condicion = 9,
                            fecha_creacion = "2020-03-25 07:35:59.8509440 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-09-20",
                    fecha_baja_bt = "1830-01-01",
                    categoria = "N",
                    canal_distribucion = 2,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = null,
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "UPD_SINCRO",
                    tipo_tributario = 2,
                    numero_tributario = "23603215729",
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
                    fecha_creacion = "2020-03-25 07:35:59.8533970 -03:00",
                    fecha_modificacion = "2021-04-30 11:56:19.5592700 -03:00"
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaDuplicadaPasaporte12,
                    pais_documento = 12,
                    tipo_documento = 10,
                    numero_documento = "31187",
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2814655,
                            fecha_creacion = "2021-08-21 07:13:20.0875790 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 2,
                            localidad = 2586,
                            localidad_maestro = "TAPIALES",
                            calle = "NAZAR 1336",
                            numero = "0",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1770,
                            codigo_postal_argentino = "C1054AAV",
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
                            id = 18776108,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0974930 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776109,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 07:13:20.0975020 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776112,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975080 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776114,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 07:13:20.0975150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776117,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975210 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776119,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975270 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776121,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975340 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-09-20",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "23000311871",
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
                    fecha_creacion = "2021-08-21 07:13:20.0874170 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaDuplicadaDniVsLibreta11,
                    pais_documento = 80,
                    tipo_documento = 5,
                    numero_documento = "8274996",
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>()
                    {
                        new TelefonoExtraInfo
                        {
                            id = 423493,
                            numero_local = 48623571,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1148623571",
                            interno = null,
                            ddn = "01148623571",
                            ddi = "+54 11 4862 3571",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-07-17 22:37:30.7511510 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 638363,
                            fecha_creacion = "2020-07-17 22:37:30.7511530 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "",
                            departamento = "9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        },
                        new Domicilio
                        {
                            id = 638365,
                            fecha_creacion = "2020-07-17 22:37:30.7511530 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "5",
                            departamento = "9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = null,
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 2394265,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498000 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394267,
                            tipo_impuesto = 2,
                            condicion = 9,
                            fecha_creacion = "2020-07-17 22:37:30.7498150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394269,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498230 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394270,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-07-17 22:37:30.7498300 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394272,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498370 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394274,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498440 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394276,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498510 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394278,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498700 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394280,
                            tipo_impuesto = 14,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498800 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2005-12-30",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 2,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "JH75618",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 1,
                    numero_tributario = "23082749969",
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
                    fecha_creacion = "2020-07-17 22:37:30.7511480 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFiltroModelOutput
                {
                    id = idPersonaDuplicadaDniVsLibreta12,
                    pais_documento = AppConstants.ArgentinaCodigoBantotal,
                    tipo_documento = (int)TipoDocumento.Dni,
                    numero_documento = nroDocumentoDuplicadoConflictoDniVsLibreta,
                    emails = new List<EmailExtraInfo>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<TelefonoExtraInfo>
                    {
                        new TelefonoExtraInfo
                        {
                            id = 757611,
                            numero_local = 40974466,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1140974466",
                            interno = null,
                            ddn = "0111540974466",
                            ddi = "+54 9 11 4097 4466",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-09-24 19:55:37.0762860 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "CELULAR",
                            dado_de_baja = false
                        },
                        new TelefonoExtraInfo
                        {
                            id = 757610,
                            numero_local = 48623571,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1148623571",
                            interno = null,
                            ddn = "01148623571",
                            ddi = "+54 11 4862 3571",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-09-24 19:55:37.0762840 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            ultima_verificacion_positiva = null,
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 958637,
                            fecha_creacion = "2020-09-24 19:55:37.0762870 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "",
                            departamento = "5 9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        },
                        new Domicilio
                        {
                            id = 958638,
                            fecha_creacion = "2020-09-24 19:55:37.0762880 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1416,
                            localidad_maestro = "CABA CP1416 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AVDA. GAONA",
                            numero = "1701",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1416,
                            codigo_postal_argentino = "C1416DRF",
                            latitud = "-34.61092755",
                            longitud = "-58.45295335",
                            normalizado = true,
                            origen_contacto = null,
                            legal = null,
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 4078352,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749210 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078353,
                            tipo_impuesto = 1,
                            condicion = 2,
                            fecha_creacion = "2020-09-24 19:55:37.0749530 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078354,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749700 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078355,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749850 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078356,
                            tipo_impuesto = 5,
                            condicion = 4,
                            fecha_creacion = "2020-09-24 19:55:37.0750000 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078357,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-09-24 19:55:37.0750160 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078358,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750490 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078359,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750650 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078360,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750820 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078361,
                            tipo_impuesto = 11,
                            condicion = 0,
                            fecha_creacion = "2020-09-24 19:55:37.0750980 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2010-11-17",
                    fecha_baja_bt = null,
                    categoria = "",
                    canal_distribucion = 0,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "SB65071",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 1,
                    numero_tributario = "23082749969",
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
                    fecha_creacion = "2020-09-24 19:55:37.0762510 -03:00",
                    fecha_modificacion = null
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
                    id = idPersonaCorrecta2,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = nroDocumentoPersonaCorrecta2,
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
                            principal = true,
                            confiable = true,
                            doble_factor = true,
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
                    id = idPersonaDuplicada11,
                    pais_documento = 80,
                    tipo_documento = 5,
                    numero_documento = "4078985",
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>
                    {
                        new TokenPush
                        {
                            id = 32242,
                            token =
                                "czB8B0y0054:APA91bE0XjG0ECOpTaT0Hxk2nlMiDrtn11X9YVozRGV5LAYmfDYIdgqgo5IKfgPQ8dN-k2v1sQT4upFSzKnvwbHDuBIzTTehtXZ1CX6cor9fidg3rspsIyUr8b32ofSdBay-he0DIOE0",
                            sistema_operativo = "ANDROID",
                            fecha_creacion = "2021-03-20 03:02:32.9841320 -03:00",
                            fecha_modificacion = "2021-08-21 13:22:05.7535840 -03:00",
                            canal_creacion = "APPJUB",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "RP16957"
                        }
                    },
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            id = 2077137,
                            numero_local = 50069999,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1150069999",
                            interno = null,
                            ddn = "0111550069999",
                            ddi = "+54 9 11 5006 9999",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2021-08-21 13:22:05.8609700 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = null,
                            tipo_telefono = "CELULAR",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 1270615,
                            fecha_creacion = "2020-12-22 20:58:31.0925140 -03:00",
                            fecha_modificacion = "2021-08-21 13:22:05.7542630 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "MW93221",
                            usuario_modificacion = "RP16957",
                            pais = 80,
                            provincia = 1,
                            localidad = 1001,
                            localidad_maestro = "CABA CP1001 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV INDEPENDENCIA",
                            numero = "1940",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1001,
                            codigo_postal_argentino = "C1225AAO",
                            latitud = "-34.6183548",
                            longitud = "-58.3938446",
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>
                    {
                        new ActividadEconomicaAfip
                        {
                            id = 638503,
                            codigo_actividad = 7,
                            descripcion_actividad = "JUBILADO",
                            orden = null,
                            fecha_actualizacion = "2021-08-21 13:22:05.7897580 -03:00"
                        }
                    },
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 6360825,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924630 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 20219784,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 13:22:05.9025400 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360827,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924850 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360828,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-12-22 20:58:31.0924900 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 20219785,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 13:22:05.9025550 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360829,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924940 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 6360830,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-12-22 20:58:31.0924990 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2009-12-10",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "MW93221",
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "RP16957",
                    tipo_tributario = 2,
                    numero_tributario = "20040789856",
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
                    fecha_creacion = "2020-12-22 20:58:31.0925130 -03:00",
                    fecha_modificacion = "2021-08-21 13:22:05.7517040 -03:00"
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaDuplicada12,
                    pais_documento = 80,
                    tipo_documento = 6,
                    numero_documento = "4078985",
                    emails = new List<Email>
                    {
                        new Email
                        {
                            id = 157618,
                            direccion = "nomail@optimspv.com",
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            fecha_creacion = "2021-07-10 08:32:44.2738870 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6575920 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            etiquetas = new List<Etiqueta>(),
                            dado_de_baja = false
                        }
                    },
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            id = 1301575,
                            numero_local = 429999,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 2352,
                            numero = "2352429999",
                            interno = null,
                            ddn = "02352429999",
                            ddi = "+54 2352 42 9999",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2021-07-10 08:32:44.2542300 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6563050 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 1828962,
                            fecha_creacion = "2021-07-10 08:32:44.2252260 -03:00",
                            fecha_modificacion = "2021-08-22 15:27:50.6587130 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = "JH75618",
                            usuario_modificacion = "RP16957",
                            pais = 80,
                            provincia = 1,
                            localidad = 1001,
                            localidad_maestro = "CABA CP1001 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV INDEPENDENCIA",
                            numero = "1940",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1001,
                            codigo_postal_argentino = "C1225AAO",
                            latitud = "-34.6183548",
                            longitud = "-58.3938446",
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>
                    {
                        new ActividadEconomicaAfip
                        {
                            id = 182795,
                            codigo_actividad = 471190,
                            descripcion_actividad =
                                "VENTA AL POR MENOR EN KIOSCOS, POLIRRUBROS Y COMERCIOS NO ESPECIALIZADOS N.C.P.",
                            orden = null,
                            fecha_actualizacion = "2021-08-22 15:27:50.6910480 -03:00"
                        }
                    },
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 9794009,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765560 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008043,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-22 15:27:50.7306880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794011,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765780 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008044,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-22 15:27:50.7306980 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 23008045,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-22 15:27:50.7307040 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794013,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2765960 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 9794014,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-07-10 08:32:44.2766050 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2007-08-15",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "JH75618",
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "RP16957",
                    tipo_tributario = 2,
                    numero_tributario = "27040789850",
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
                    fecha_creacion = "2021-07-10 08:32:44.2250420 -03:00",
                    fecha_modificacion = "2021-08-22 15:27:50.6474780 -03:00"
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaDuplicadaPasaporte11,
                    pais_documento = 4,
                    tipo_documento = 10,
                    numero_documento = "31187",
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            id = 70314,
                            numero_local = 43220068,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1143220068",
                            interno = null,
                            ddn = "01143220068",
                            ddi = "+54 11 4322 0068",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-03-25 07:35:59.8534020 -03:00",
                            fecha_modificacion = "2021-04-30 11:56:19.5592780 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "UPD_SINCRO",
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 47350,
                            fecha_creacion = "2020-03-25 07:35:59.8534050 -03:00",
                            fecha_modificacion = "2021-04-30 11:56:19.5602860 -03:00",
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = "BANTOTAL",
                            usuario_creacion = null,
                            usuario_modificacion = "UPD_SINCRO",
                            pais = 80,
                            provincia = 1,
                            localidad = 1054,
                            localidad_maestro = "CABA CP1054 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AV CORDOBA",
                            numero = "946",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1054,
                            codigo_postal_argentino = "C1054AAV",
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
                            id = 171509,
                            tipo_impuesto = 14,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510330 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171508,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510180 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171507,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8510030 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171506,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8509880 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171505,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-03-25 07:35:59.8509730 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171504,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-03-25 07:35:59.8509590 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 171503,
                            tipo_impuesto = 2,
                            condicion = 9,
                            fecha_creacion = "2020-03-25 07:35:59.8509440 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-09-20",
                    fecha_baja_bt = "1830-01-01",
                    categoria = "N",
                    canal_distribucion = 2,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = null,
                    canal_modificacion = "BANTOTAL",
                    usuario_modificacion = "UPD_SINCRO",
                    tipo_tributario = 2,
                    numero_tributario = "23603215729",
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
                    fecha_creacion = "2020-03-25 07:35:59.8533970 -03:00",
                    fecha_modificacion = "2021-04-30 11:56:19.5592700 -03:00"
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaDuplicadaPasaporte12,
                    pais_documento = 12,
                    tipo_documento = 10,
                    numero_documento = "31187",
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>(),
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 2814655,
                            fecha_creacion = "2021-08-21 07:13:20.0875790 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "RP16957",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 2,
                            localidad = 2586,
                            localidad_maestro = "TAPIALES",
                            calle = "NAZAR 1336",
                            numero = "0",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1770,
                            codigo_postal_argentino = "C1054AAV",
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
                            id = 18776108,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0974930 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776109,
                            tipo_impuesto = 2,
                            condicion = 99,
                            fecha_creacion = "2021-08-21 07:13:20.0975020 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776112,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975080 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776114,
                            tipo_impuesto = 5,
                            condicion = 5,
                            fecha_creacion = "2021-08-21 07:13:20.0975150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776117,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975210 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776119,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975270 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 18776121,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2021-08-21 07:13:20.0975340 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2004-09-20",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 22,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "RP16957",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 2,
                    numero_tributario = "23000311871",
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
                    fecha_creacion = "2021-08-21 07:13:20.0874170 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaDuplicadaDniVsLibreta11,
                    pais_documento = 80,
                    tipo_documento = 5,
                    numero_documento = nroDocumentoDuplicadoConflictoDniVsLibreta,
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>()
                    {
                        new Telefono
                        {
                            id = 423493,
                            numero_local = 48623571,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1148623571",
                            interno = null,
                            ddn = "01148623571",
                            ddi = "+54 11 4862 3571",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-07-17 22:37:30.7511510 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 638363,
                            fecha_creacion = "2020-07-17 22:37:30.7511530 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "",
                            departamento = "9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        },
                        new Domicilio
                        {
                            id = 638365,
                            fecha_creacion = "2020-07-17 22:37:30.7511530 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "JH75618",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "5",
                            departamento = "9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = null,
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 2394265,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498000 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394267,
                            tipo_impuesto = 2,
                            condicion = 9,
                            fecha_creacion = "2020-07-17 22:37:30.7498150 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394269,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498230 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394270,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-07-17 22:37:30.7498300 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394272,
                            tipo_impuesto = 6,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498370 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394274,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498440 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394276,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498510 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394278,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498700 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 2394280,
                            tipo_impuesto = 14,
                            condicion = 1,
                            fecha_creacion = "2020-07-17 22:37:30.7498800 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2005-12-30",
                    fecha_baja_bt = null,
                    categoria = "N",
                    canal_distribucion = 2,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "JH75618",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 1,
                    numero_tributario = "23082749969",
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
                    fecha_creacion = "2020-07-17 22:37:30.7511480 -03:00",
                    fecha_modificacion = null
                },
                new ApiPersonasFisicaInfoModelOutput
                {
                    id = idPersonaDuplicadaDniVsLibreta12,
                    pais_documento = 80,
                    tipo_documento = 4,
                    numero_documento = nroDocumentoDuplicadoConflictoDniVsLibreta,
                    emails = new List<Email>(),
                    tokens_push = new List<TokenPush>(),
                    telefonos = new List<Telefono>
                    {
                        new Telefono
                        {
                            id = 757611,
                            numero_local = 40974466,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1140974466",
                            interno = null,
                            ddn = "0111540974466",
                            ddi = "+54 9 11 4097 4466",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-09-24 19:55:37.0762860 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            tipo_telefono = "CELULAR",
                            dado_de_baja = false
                        },
                        new Telefono
                        {
                            id = 757610,
                            numero_local = 48623571,
                            pais = 80,
                            prefijo_telefonico_pais = 54,
                            codigo_area = 11,
                            numero = "1148623571",
                            interno = null,
                            ddn = "01148623571",
                            ddi = "+54 11 4862 3571",
                            no_llame = "false",
                            fecha_alta_no_llame = null,
                            fecha_baja_no_llame = null,
                            es_geografico = true,
                            nombre_interlocutor = null,
                            cargo_interlocutor = null,
                            score = null,
                            normalizado = true,
                            fecha_creacion = "2020-09-24 19:55:37.0762840 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            compania = null,
                            origen_contacto = "TERCERO",
                            principal = null,
                            confiable = null,
                            doble_factor = null,
                            etiquetas = new List<Etiqueta>(),
                            tipo_telefono = "FIJO",
                            dado_de_baja = false
                        }
                    },
                    domicilios = new List<Domicilio>
                    {
                        new Domicilio
                        {
                            id = 958637,
                            fecha_creacion = "2020-09-24 19:55:37.0762870 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1185,
                            localidad_maestro = "CABA CP1185 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "LAMBARE",
                            numero = "1099",
                            piso = "",
                            departamento = "5 9",
                            codigo_postal = 1185,
                            codigo_postal_argentino = "C1185ABE",
                            latitud = null,
                            longitud = null,
                            normalizado = true,
                            origen_contacto = null,
                            legal = "true",
                            localidad_alfanumerica = null
                        },
                        new Domicilio
                        {
                            id = 958638,
                            fecha_creacion = "2020-09-24 19:55:37.0762880 -03:00",
                            fecha_modificacion = null,
                            canal_creacion = "BANTOTAL",
                            canal_modificacion = null,
                            usuario_creacion = "SB65071",
                            usuario_modificacion = null,
                            pais = 80,
                            provincia = 1,
                            localidad = 1416,
                            localidad_maestro = "CABA CP1416 CIUDAD AUTONOMA DE BUENOS AIRES",
                            calle = "AVDA. GAONA",
                            numero = "1701",
                            piso = "",
                            departamento = "",
                            codigo_postal = 1416,
                            codigo_postal_argentino = "C1416DRF",
                            latitud = "-34.61092755",
                            longitud = "-58.45295335",
                            normalizado = true,
                            origen_contacto = null,
                            legal = null,
                            localidad_alfanumerica = null
                        }
                    },
                    documentos_adicionales = new List<DocumentoAdicional>(),
                    actividades_economicas_afip = new List<ActividadEconomicaAfip>(),
                    impuestos = new List<Impuesto>
                    {
                        new Impuesto
                        {
                            id = 4078352,
                            tipo_impuesto = 1,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749210 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078353,
                            tipo_impuesto = 1,
                            condicion = 2,
                            fecha_creacion = "2020-09-24 19:55:37.0749530 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078354,
                            tipo_impuesto = 2,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749700 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078355,
                            tipo_impuesto = 4,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0749850 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078356,
                            tipo_impuesto = 5,
                            condicion = 4,
                            fecha_creacion = "2020-09-24 19:55:37.0750000 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078357,
                            tipo_impuesto = 5,
                            condicion = 9,
                            fecha_creacion = "2020-09-24 19:55:37.0750160 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078358,
                            tipo_impuesto = 8,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750490 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078359,
                            tipo_impuesto = 9,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750650 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078360,
                            tipo_impuesto = 10,
                            condicion = 1,
                            fecha_creacion = "2020-09-24 19:55:37.0750820 -03:00",
                            fecha_modificacion = null
                        },
                        new Impuesto
                        {
                            id = 4078361,
                            tipo_impuesto = 11,
                            condicion = 0,
                            fecha_creacion = "2020-09-24 19:55:37.0750980 -03:00",
                            fecha_modificacion = null
                        }
                    },
                    tipo_persona = "F",
                    fecha_alta_bt = "2010-11-17",
                    fecha_baja_bt = null,
                    categoria = "",
                    canal_distribucion = 0,
                    canal_creacion = "BANTOTAL",
                    usuario_creacion = "SB65071",
                    canal_modificacion = null,
                    usuario_modificacion = null,
                    tipo_tributario = 1,
                    numero_tributario = "23082749969",
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
                    fecha_creacion = "2020-09-24 19:55:37.0762510 -03:00",
                    fecha_modificacion = null
                }
            };

        public WireMockHelper WireMockHelper { get; set; }

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        public ValidacionExistenciaIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        private static ServiceRequest ValidarExistencia(
            Uri uriBase,
            string nroDocumento,
            int? tipoDocumento = null,
            int? paisId = null)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionExistencia(nroDocumento, tipoDocumento, paisId));

            return ServiceRequest.Get(uri.AbsoluteUri);
        }

        [Theory]
        [MemberData(nameof(Datos))]
        public async Task ValidarExistenciaAsync(
            string nroDocumento,
            bool migrado,
            string usuario,
            HttpStatusCode httpStatusCode,
            HttpStatusCode statusCodeValidarExistencia)
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathPersonaFisicaInexistente =
                $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaFisicaInexistente)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumento}_{statusCodeValidarExistencia}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ValidacionExistenciaModelOutput
                    {
                        Migrado = migrado,
                        Usuario = usuario,
                        IdEstadoUsuario = (int)UserStatus.Active
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("ValidarExistenciaAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta)
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("ValidarExistenciaAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaCorrecta)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaInexistente))
                .WithTitle("ValidarExistenciaAsync-PersonaFisicaInexistente")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaFisicaInexistente)
                    }));

            var request = ValidarExistencia(_uriBase, nroDocumento);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(httpStatusCode);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaUsuarioMigradoSinTelefonoOkAsync()
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoPersonaCorrecta}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = true,
                        id_estado_usuario = (int)UserStatus.Active
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioMigradoSinTelefonoOkAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta)
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioMigradoSinTelefonoOkAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaCorrecta)));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoPersonaCorrecta);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(true);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(idPersonaCorrecta);
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Active);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaUsuarioNoMigradoSinTelefonoOkAsync()
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoPersonaCorrecta}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioNoMigradoSinTelefonoOkAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta)
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioNoMigradoSinTelefonoOkAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaCorrecta)));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoPersonaCorrecta);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(idPersonaCorrecta);
            result.IdEstadoUsuario.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaUsuarioMigradoConTelefonoOkAsync()
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta2)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta2)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoPersonaCorrecta2}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = true,
                        id_estado_usuario = (int)UserStatus.Active
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioMigradoConTelefonoOkAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta2)
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioMigradoConTelefonoOkAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaCorrecta2)));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoPersonaCorrecta2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(true);
            result.Telefono.Should().Be("1123454356");
            result.IdPersona.Should().Be(idPersonaCorrecta2);
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Active);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaUsuarioNoMigradoConTelefonoOkAsync()
        {
            // Arrange
            var pathPersonaCorrecta = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoPersonaCorrecta2)}";
            var pathPersonaFisicaPersonaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta2)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoPersonaCorrecta2}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false,
                        id_estado_usuario = (int)UserStatus.Blocked
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioNoMigradoConTelefonoOkAsync-PersonaCorrecta")
                .RespondWith(WireMockHelper.Json(
                    new List<ApiPersonasFiltroModelOutput>
                    {
                        ApiPersonasFiltroModelOutput.First(x => x.id == idPersonaCorrecta2)
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaPersonaCorrecta))
                .WithTitle("ValidarExistenciaUsuarioNoMigradoConTelefonoOkAsync-PersonaFisicaPersonaCorrecta")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaCorrecta2)));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoPersonaCorrecta2);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("1123454356");
            result.IdPersona.Should().Be(idPersonaCorrecta2);
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Blocked);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicado = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaDuplicada11 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada11)}";
            var pathPersonaFisicaDuplicada12 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada12)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicado))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaNroDocDuplicado")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x => x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc)
                        .ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada11)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada12))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada12)));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoDuplicadoConflictoTipoDoc);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(0);
            result.Usuario.Should().Be("");
            result.IdEstadoUsuario.Should().Be(0);
            result.Conflicto.Should().Be(true);
            result.TiposDocumento.Count.Should().Be(2);
            result.TiposDocumento[0].Codigo.Should().Be(idTipoDocumentoDuplicado11);
            result.TiposDocumento[0].Descripcion.Should().Be(AppConstants.LibretaEnrolamiento);
            result.TiposDocumento[1].Codigo.Should().Be(IdTipoDocumentoDuplicado12);
            result.TiposDocumento[1].Descripcion.Should().Be(AppConstants.LibretaCivica);
            result.Paises.Count.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoPasandoTipoDocMasPaisIdOkAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicado = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaDuplicada11 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada11)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false,
                        id_estado_usuario = (int)UserStatus.Inactive
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicado))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoPasandoTipoDocMasPaisIdOkAsync-PersonaNroDocDuplicado")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x =>
                            x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc)
                        .ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoPasandoTipoDocMasPaisIdOkAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada11)));

            // Act
            var request = ValidarExistencia(
                _uriBase,
                nroDocumentoDuplicadoConflictoTipoDoc,
                idTipoDocumentoDuplicado11,
                AppConstants.ArgentinaCodigoBantotal);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(idPersonaDuplicada11);
            result.Usuario.Should().Be("");
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Inactive);
            result.Conflicto.Should().Be(false);
            result.TiposDocumento.Count.Should().Be(0);
            result.Paises.Count.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoPasandoTipoDocOkAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicado = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaDuplicada11 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada11)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false,
                        id_estado_usuario = (int)UserStatus.Migrated
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicado))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoPasandoTipoDocOkAsync-PersonaNroDocDuplicado")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x =>
                            x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc)
                        .ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoPasandoTipoDocOkAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada11)));

            // Act
            var request = ValidarExistencia(
                _uriBase,
                nroDocumentoDuplicadoConflictoTipoDoc,
                idTipoDocumentoDuplicado11);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(idPersonaDuplicada11);
            result.Usuario.Should().Be("");
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Migrated);
            result.Conflicto.Should().Be(false);
            result.TiposDocumento.Count.Should().Be(0);
            result.Paises.Count.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoOkAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicado = $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoTipoDoc)}";
            var pathPersonaFisicaDuplicada11 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada11)}";
            var pathPersonaFisicaDuplicada12 = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicada12)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoTipoDoc}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false
                    }));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicado))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoOkAsync-PersonaNroDocDuplicado")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x =>
                        x.numero_documento == nroDocumentoDuplicadoConflictoTipoDoc).ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoOkAsync-PersonaFisicaDuplicada11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada11)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicada12))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDeTiposDocumentoOkAsync-PersonaFisicaDuplicada12")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicada12)));

            // Act
            var request = ValidarExistencia(
                _uriBase,
                nroDocumentoDuplicadoConflictoTipoDoc,
                (int)TipoDocumento.Le,
                AppConstants.ArgentinaCodigoBantotal);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(15202077);
            result.Usuario.Should().Be("");
            result.IdEstadoUsuario.Should().Be(0);
            result.Conflicto.Should().Be(false);
            result.TiposDocumento.Count.Should().Be(0);
            result.Paises.Count.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoConflictoDePaisAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicadoPasaporte =
                $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoPais)}";
            var pathPersonaFisicaDuplicadaPasaporte11 =
                $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicadaPasaporte11)}";
            var pathPersonaFisicaDuplicadaPasaporte12 =
                $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicadaPasaporte12)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";
            var pathCatalogoPaises = $"{ApiCatalogoUris.Paises()}";

            var paises = new List<ApiCatalogoPaisesModelOutput>
            {
                new ApiCatalogoPaisesModelOutput { codigo = 4, descripcion = "FRANCIA REPUBLICA" },
                new ApiCatalogoPaisesModelOutput { codigo = 12, descripcion = "BRASIL, REPUBLICA FEDERATIVA" },
                new ApiCatalogoPaisesModelOutput { codigo = 80, descripcion = "ARGENTINA, REPUBLICA" }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathCatalogoPaises))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDePaisAsync-CatalogoPaises")
                .RespondWith(WireMockHelper.Json(paises));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicadoPasaporte))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDePaisAsync-PersonaNroDocDuplicadoPasaporte")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x =>
                            x.numero_documento == nroDocumentoDuplicadoConflictoPais)
                        .ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadaPasaporte11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDePaisAsync-PersonaFisicaDuplicadaPasaporte11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicadaPasaporte11)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadaPasaporte12))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDePaisAsync-PersonaFisicaDuplicadaPasaporte12")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicadaPasaporte12)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoPais}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = false
                    }));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoDuplicadoConflictoPais);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(false);
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(0);
            result.IdEstadoUsuario.Should().Be(0);
            result.Conflicto.Should().Be(true);
            result.TiposDocumento.Count.Should().Be(0);
            result.Paises.Count.Should().Be(2);
            result.Paises[0].Codigo.Should().Be(4);
            result.Paises[0].Descripcion.Should().Be("FRANCIA REPUBLICA");
            result.Paises[1].Codigo.Should().Be(12);
            result.Paises[1].Descripcion.Should().Be("BRASIL, REPUBLICA FEDERATIVA");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task ValidarExistenciaDocumentoDuplicadoConflictoDniVsLibretasAsync()
        {
            // Arrange
            var pathPersonaNroDocDuplicadoDniVsLibreta =
                $"{ApiPersonasUris.PersonasFiltro(nroDocumentoDuplicadoConflictoDniVsLibreta)}";
            var pathPersonaFisicaDuplicadaDniVsLibreta11 =
                $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicadaDniVsLibreta11)}";
            var pathPersonaFisicaDuplicadaDniVsLibreta12 =
                $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaDuplicadaDniVsLibreta12)}";
            var pathValidacionExistencia = $"{ApiUsuariosUris.ValidarExistencia()}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaNroDocDuplicadoDniVsLibreta))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDniVsLibretasAsync-PersonaNroDocDuplicadoDniVsLibreta")
                .RespondWith(WireMockHelper.Json(
                    ApiPersonasFiltroModelOutput.Where(x =>
                            x.numero_documento == nroDocumentoDuplicadoConflictoDniVsLibreta)
                        .ToList()
                ));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadaDniVsLibreta11))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDniVsLibretasAsync-PersonaFisicaDuplicadaDniVsLibreta11")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicadaDniVsLibreta11)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaDuplicadaDniVsLibreta12))
                .WithTitle("ValidarExistenciaDocumentoDuplicadoConflictoDniVsLibretasAsync-PersonaFisicaDuplicadaDniVsLibreta12")
                .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x =>
                    x.id == idPersonaDuplicadaDniVsLibreta12)));

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Post(pathValidacionExistencia))
                .WithTitle($"{nroDocumentoDuplicadoConflictoDniVsLibreta}_{HttpStatusCode.OK}_validar_existencia")
                .RespondWith(WireMockHelper.Json(
                    new ApiUsuariosValidacionExistenciaModelOutput
                    {
                        migrado = true,
                        usuario = "",
                        id_estado_usuario = (int)UserStatus.Active
                    }));

            // Act
            var request = ValidarExistencia(_uriBase, nroDocumentoDuplicadoConflictoDniVsLibreta);

            // Act
            var sut = await SendAsync(request);

            // Assert
            sut.IsSuccessStatusCode.Should().BeTrue();

            var result = await sut.Content.ReadAsAsync<ValidacionExistenciaModelResponse>();

            result.Should().NotBeNull();
            result.Migrado.Should().Be(true);
            result.Usuario.Should().Be("");
            result.Telefono.Should().Be("");
            result.IdPersona.Should().Be(idPersonaDuplicadaDniVsLibreta11);
            result.IdEstadoUsuario.Should().Be((int)UserStatus.Active);
            result.Conflicto.Should().Be(false);
            result.TiposDocumento.Count.Should().Be(0);
            result.Paises.Count.Should().Be(0);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}