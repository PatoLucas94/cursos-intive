using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Input;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Registración model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegistracionModelRequest
    {
        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersona))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Identificador de País.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPais), DomainValidation(typeof(IdPais))]
        [JsonProperty(PropertyName = ParameterNames.IdPais)]
        public int IdPais { get; set; }

        /// <summary>
        /// Identificador de Tipo de Documento.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdTipoDocumento), DomainValidation(typeof(IdTipoDocumento))]
        [JsonProperty(PropertyName = ParameterNames.IdTipoDocumento)]
        public int IdTipoDocumento { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        [FromHeader(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Usuario)]
        public string Usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Clave)]
        public string Clave { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [FromHeader(Name = ParameterNames.Email), DomainValidation(typeof(Email))]
        [JsonProperty(PropertyName = ParameterNames.Email)]
        public string Email { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [FromHeader(Name = ParameterNames.Telefono), DomainValidation(typeof(Telefono))]
        [JsonProperty(PropertyName = ParameterNames.Telefono)]
        public string Telefono { get; set; }

        /// <summary>
        /// SmsValidado
        /// </summary>
        [FromHeader(Name = ParameterNames.SmsValidado), DomainValidation(typeof(SmsValidado))]
        [JsonProperty(PropertyName = ParameterNames.SmsValidado)]
        public bool SmsValidado { get; set; }

        /// <summary>
        /// Clave de Canales
        /// </summary>
        [FromHeader(Name = ParameterNames.ClaveCanales), DomainValidation(typeof(ClaveCanales))]
        [JsonProperty(PropertyName = ParameterNames.ClaveCanales)]
        public string ClaveCanales { get; set; }

        /// <summary>
        /// Identificador de Términos y Condiciones.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdTyC), DomainValidation(typeof(IdTyC))]
        [JsonProperty(PropertyName = ParameterNames.IdTyC)]
        public string IdTyC { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con Clave ofuscada
        /// </summary>
        public override string ToString()
        {
            return "{ IdPersona: '" + IdPersona + "', IdPais:'" + IdPais + "', IdTipoDocumento:'" + IdTipoDocumento
                   + "', NroDocumento:'" + NroDocumento + "', Usuario:'************', Clave: '************'"
                   + "', Email:'" + Email + "', Telefono:'" + Telefono + "', SmsValidado:'" + SmsValidado
                   + "'ClaveCanales: '************'" + "', IdTyC:'" + IdTyC + "}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<RegistracionModelInput> ToRequestBody(ApiHeadersDeviceId headers)
        {
            return headers?.ToRequestBody(
                new RegistracionModelInput
                {
                    PersonId = IdPersona,
                    DocumentCountryId = IdPais,
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NroDocumento,
                    UserName = Usuario,
                    Password = Clave,
                    Email = Email,
                    Phone = Telefono,
                    SmsValidated = SmsValidado,
                    ChannelKey = ClaveCanales,
                    TyCId = IdTyC,
                    DeviceId = headers.DeviceId
                });
        }
    }
}
