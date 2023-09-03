using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.SoftToken.Input;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;

namespace Spv.Usuarios.Bff.ViewModels.SoftToken.Input
{
    /// <summary>
    /// SoftTokenModelRequest
    /// </summary>
    public class SoftTokenModelRequest
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [FromQuery(Name = ParameterNames.Identificador), Required(ErrorMessage = MessageConstants.SoftTokenIdentificadorInvalido), MinLength(1)]
        public string Identificador { get; set; }
        /// <summary>
        /// ToRequestBody
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public IRequestBody<SoftTokenModelInput> ToRequestBody(ApiHeaders headers)
        {
            return headers?.ToRequestBody(new SoftTokenModelInput
            {
                Identificador = Identificador
            });
        }
    }
}
