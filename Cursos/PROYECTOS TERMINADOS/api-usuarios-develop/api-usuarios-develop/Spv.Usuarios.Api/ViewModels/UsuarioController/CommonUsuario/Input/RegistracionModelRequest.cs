using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Api.Common.Attributes;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Api.ViewModels.CommonController.Input;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Domain.ApiEntities;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Input
{
    /// <summary>
    /// Registración model v1
    /// </summary>
    public class RegistracionModelRequest
    {
        /// <summary>
        /// Número de Cliente
        /// </summary>
        [FromHeader(Name = ParameterNames.NroCliente), DomainValidation(typeof(NroCliente))]
        [JsonProperty(PropertyName = ParameterNames.NroCliente)]
        public string NroCliente { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [FromHeader(Name = ParameterNames.Usuario), DomainValidation(typeof(CredUsuarioOriginal))]
        [JsonProperty(PropertyName = ParameterNames.Usuario)]
        public string Usuario { get; set; }

        /// <summary>
        /// Clave
        /// </summary>
        [FromHeader(Name = ParameterNames.Clave), DomainValidation(typeof(CredClaveOriginal))]
        [JsonProperty(PropertyName = ParameterNames.Clave)]
        public string Clave { get; set; }

        /// <summary>
        /// Identificador de Estado de Usuario.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdEstadoUsuario), DomainValidation(typeof(IdEstadoUsuario))]
        [JsonProperty(PropertyName = ParameterNames.IdEstadoUsuario)]
        public int IdEstadoUsuario { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [FromHeader(Name = ParameterNames.Nombre), DomainValidation(typeof(Nombre))]
        [JsonProperty(PropertyName = ParameterNames.Nombre)]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [FromHeader(Name = ParameterNames.Apellido), DomainValidation(typeof(Apellido))]
        [JsonProperty(PropertyName = ParameterNames.Apellido)]
        public string Apellido { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [FromHeader(Name = ParameterNames.Email), DomainValidation(typeof(Email))]
        [JsonProperty(PropertyName = ParameterNames.Email)]
        public string Email { get; set; }

        /// <summary>
        /// Teléfono Laboral
        /// </summary>
        [FromHeader(Name = ParameterNames.TelefonoLaboral), DomainValidation(typeof(Telefono))]
        [JsonProperty(PropertyName = ParameterNames.TelefonoLaboral)]
        public string TelefonoLaboral { get; set; }

        /// <summary>
        /// Celular
        /// </summary>
        [FromHeader(Name = ParameterNames.Celular), DomainValidation(typeof(Telefono))]
        [JsonProperty(PropertyName = ParameterNames.Celular)]
        public string Celular { get; set; }

        /// <summary>
        /// Identificador de País.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPais), DomainValidation(typeof(IdPais))]
        [JsonProperty(PropertyName = ParameterNames.IdPais)]
        public string IdPais { get; set; }

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
        /// Cuil
        /// </summary>
        [FromHeader(Name = ParameterNames.Cuil), DomainValidation(typeof(Cuil))]
        [JsonProperty(PropertyName = ParameterNames.Cuil)]
        public string Cuil { get; set; }

        /// <summary>
        /// Extracto de Recibo.
        /// </summary>
        [FromHeader(Name = ParameterNames.ExtractoDeRecibo), DomainValidation(typeof(ExtractoDeRecibo))]
        [JsonProperty(PropertyName = ParameterNames.ExtractoDeRecibo)]
        public bool? ExtractoDeRecibo { get; set; }
        
        /// <summary>
        /// Control Full.
        /// </summary>
        [FromHeader(Name = ParameterNames.ControlFull), DomainValidation(typeof(ControlFull))]
        [JsonProperty(PropertyName = ParameterNames.ControlFull)]
        public bool? ControlFull { get; set; }

        /// <summary>
        /// Identificador de Compañía de Celular.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdCompaniaCelular), DomainValidation(typeof(IdCompaniaCelular))]
        [JsonProperty(PropertyName = ParameterNames.IdCompaniaCelular)]
        public int IdCompaniaCelular { get; set; }

        /// <summary>
        /// Recibe SMS.
        /// </summary>
        [FromHeader(Name = ParameterNames.RecibeSms), DomainValidation(typeof(RecibeSms))]
        [JsonProperty(PropertyName = ParameterNames.RecibeSms)]
        public bool? RecibeSms { get; set; }

        /// <summary>
        /// Recibe Mail.
        /// </summary>
        [FromHeader(Name = ParameterNames.RecibeMail), DomainValidation(typeof(RecibeMail))]
        [JsonProperty(PropertyName = ParameterNames.RecibeMail)]
        public bool? RecibeMail { get; set; }

        /// <summary>
        /// Identificador de Persona.
        /// </summary>
        [FromHeader(Name = ParameterNames.IdPersona), DomainValidation(typeof(IdPersonaOriginal))]
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public string IdPersona { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto con clave y usuario ofuscados
        /// </summary>
        public override string ToString()
        {
            return $"{{ {ParameterNames.NroCliente}:'{NroCliente}', {ParameterNames.Usuario}"+
               $":'************',{ParameterNames.Clave}:'************', { ParameterNames.IdEstadoUsuario }:'"+
               $"{IdEstadoUsuario}', {ParameterNames.Nombre}:'{Nombre}', {ParameterNames.Apellido}:'"+
               $"{Apellido}', {ParameterNames.Email}:'{ Email}', {ParameterNames.TelefonoLaboral}:'"+
               $"{TelefonoLaboral}', { ParameterNames.Celular }:'{Celular}',{ParameterNames.IdPais}:'"+
               $"{IdPais}', {ParameterNames.IdTipoDocumento}:'{IdTipoDocumento}', {ParameterNames.NroDocumento}:'"+
               $"{NroDocumento}', {ParameterNames.Cuil}:'{Cuil}', {ParameterNames.ExtractoDeRecibo}:'"+
               $"{ExtractoDeRecibo}', {ParameterNames.ControlFull}:'{ControlFull}', {ParameterNames.IdCompaniaCelular}:'"+
               $"{IdCompaniaCelular}', {ParameterNames.RecibeSms}:'{RecibeSms}', {ParameterNames.RecibeMail}:'"+
               $"{RecibeMail}', {ParameterNames.IdPersona}:'{IdPersona}'}}";
        }

        /// <summary>
        /// ToRequestBody
        /// </summary>
        public IRequestBody<RegistracionModelInput> ToRequestBody(ApiHeaders headers, IAllowedChannels allowedChannels)
        {
            return headers?.ToRequestBody(
                new RegistracionModelInput
                {
                    CustomerNumber = NroCliente,
                    UserName = Usuario,
                    Password = Clave,
                    UserStatusId = IdEstadoUsuario,
                    FirstName = Nombre,
                    LastName = Apellido,
                    Mail = Email,
                    WorkPhone = TelefonoLaboral,
                    CellPhone = Celular,
                    DocumentCountryId = IdPais,
                    DocumentTypeId = IdTipoDocumento,
                    DocumentNumber = NroDocumento,
                    Cuil = Cuil,
                    ReceiptExtract = ExtractoDeRecibo,
                    FullControl = ControlFull,
                    CellCompanyId = IdCompaniaCelular,
                    ReceiveSms = RecibeSms,
                    ReceiveMail = RecibeMail,
                    PersonId = IdPersona
                }, allowedChannels);
        }
    }
}
