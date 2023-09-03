using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Attributes;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Domain.ApiEntities;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.TyCController.CommonTyC.Input
{
    /// <summary>
    /// PersonaModelRequest
    /// </summary>
    [JsonObject(Title = "TyCPersonaModelRequest")]
    public class TyCPersonaModelRequest
    {
        /// <summary>
        /// Usuario
        /// </summary>
        [FromQuery(Name = ParameterNames.Usuario)]
        public string Usuario { get; set; }

        /// <summary>
        /// Identificador de número de documento
        /// </summary>
        [FromQuery(Name = ParameterNames.NroDocumento), DomainValidation(typeof(NroDocumento))]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Retorna la representación en json del objeto
        /// </summary>
        public override string ToString() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <returns></returns>
        public IRequestBody<(string NumeroDocumento, string Usuario)> ToRequestBody(ApiHeaders headers) =>
            headers?.ToRequestBody((NumeroDocumento, Usuario));
    }
}
