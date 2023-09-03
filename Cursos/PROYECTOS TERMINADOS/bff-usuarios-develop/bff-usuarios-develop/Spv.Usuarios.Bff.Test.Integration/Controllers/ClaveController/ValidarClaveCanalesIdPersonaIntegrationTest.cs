using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.ExternalResponses;
using Spv.Usuarios.Bff.Test.Infrastructure;
using Spv.Usuarios.Bff.Test.Integration.ExternalServices.ApiUris;
using Spv.Usuarios.Bff.ViewModels.ClaveController.CommonClave.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Errors;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Integration.Controllers.ClaveController
{
    [Collection(ServerFixtureIntegrationCollection.Name)]
    public class ValidarClaveCanalesIdPersonaIntegrationTest : ControllerIntegrationTest
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

        private static readonly int idPaisPersonaCorrecta = 80;
        private static readonly int idTipoDocumentoPersonaCorrecta = 4;
        private static readonly string nroDocumentoPersonaCorrecta = "23734745";
        private static readonly string nroDocumentoPersonaCorrecta2 = "12345678";
        private static readonly int idPersonaCorrecta = 14155917;
        private static readonly int idPersonaCorrecta2 = 11112222;
        private static readonly int idPersonaFisicaInexistente = 1;

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
            },

        };

        protected override IEnumerable<ServiceRequest> AllRequests => throw new NotImplementedException();

        private static ServiceRequest PostValidarClaveCanales(Uri uriBase, ValidacionClaveCanalesIdPersonaModelRequest body)
        {
            var uri = new Uri(uriBase, ApiUris.ValidacionClaveCanalesConIdPersona);

            return ServiceRequest.Post(uri.AbsoluteUri, body);
        }

        public ValidarClaveCanalesIdPersonaIntegrationTest(ServerFixture server) : base(server)
        {
            _uriBase = server.HttpServer.TestServer.BaseAddress;
            WireMockHelper = server.WireMock;
        }

        [Fact]
        public async Task CanalesAsyncOk()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";
            var pathPersonaFisicaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("CanalesAsyncOk")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPersonaFisicaCorrecta))
                   .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)));

            var request = PostValidarClaveCanales(
                _uriBase,
                new ValidacionClaveCanalesIdPersonaModelRequest
                {
                    ClaveCanales = "12345678",
                    IdPersona = idPersonaCorrecta.ToString()
                });

            // Act
            var sut = await SendAsync(request, requestId: "SuccessUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status202Accepted);

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task CanalesAsyncError()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";
            var pathPersonaFisicaCorrecta = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaCorrecta)}";

            var response = new ApiUsuariosErrorResponse
            {
                Errores = new List<ApiUsuarioError>
                {
                    new ApiUsuarioError("NXE", "", "", "Error de prueba", "")
                }
            };

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, ErrorHeaders))
                .WithTitle("CanalesAsyncError")
                .RespondWith(WireMockHelper.RespondWithUnauthorized(response));

            WireMockHelper.ServiceMock
                   .Given(WireMockHelper.Get(pathPersonaFisicaCorrecta))
                   .RespondWith(WireMockHelper.Json(ApiPersonaFisicaInfoOutput.First(x => x.id == idPersonaCorrecta)));

            var request = PostValidarClaveCanales(
                _uriBase,
                new ValidacionClaveCanalesIdPersonaModelRequest
                {
                    ClaveCanales = "12345678",
                    IdPersona = idPersonaCorrecta.ToString()
                });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();
            result.Errores.First().Detalle.Should().Be("Error de prueba");
            result.Errores.First().Codigo.Should().Be("NXE");

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }

        [Fact]
        public async Task CanalesAsyncErrorPersonaFisicaInexistente()
        {
            // Arrange
            var path = $"{ApiUsuariosUris.ValidacionClaveCanales()}";
            var pathPersonaFisicaInexistente = $"{ApiPersonasUris.PersonasFisicaInfo(idPersonaFisicaInexistente)}";

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.PostWithHeaders(path, SuccessHeaders))
                .WithTitle("CanalesAsyncErrorPersonaFisicaInexistente-ValidacionClaveCanales")
                .RespondWith(WireMockHelper.RespondWithAccepted());

            WireMockHelper.ServiceMock
                .Given(WireMockHelper.Get(pathPersonaFisicaInexistente))
                .WithTitle("CanalesAsyncErrorPersonaFisicaInexistente-PersonasFisicaInfo")
                .RespondWith(WireMockHelper.RespondWithNotFound());

            var request = PostValidarClaveCanales(
                _uriBase,
                new ValidacionClaveCanalesIdPersonaModelRequest
                {
                    ClaveCanales = "12345678",
                    IdPersona = idPersonaFisicaInexistente.ToString()
                });

            // Act
            var sut = await SendAsync(request, requestId: "ErrorUser");

            // Assert
            sut.Should().NotBeNull();
            sut.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = await sut.Content.ReadAsAsync<ApiUsuariosErrorResponse>();

            result.Errores.First().Detalle.Should().Be(MessageConstants.PersonaFisicaInexistente(idPersonaFisicaInexistente));

            // Cleaning WireMock
            WireMockHelper.ServiceMock.ResetMappings();
        }
    }
}
