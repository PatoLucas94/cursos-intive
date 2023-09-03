using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// ValidacionExistenciaModelResponse
    /// </summary>
    public class ValidacionExistenciaModelResponse
    {
        /// <summary>
        /// Telefono
        /// </summary>
        [JsonProperty(PropertyName = "telefono")]
        public string Telefono { get; set; }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = "id_persona")]
        public long IdPersona { get; set; }

        /// <summary>
        /// Usuario migrado
        /// </summary>
        [JsonProperty(PropertyName = "migrado")]
        public bool Migrado { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [JsonProperty(PropertyName = "usuario")]
        public string Usuario { get; set; }

        /// <summary>
        /// Id Estado del Usuario
        /// </summary>
        [JsonProperty(PropertyName = "id_estado_usuario")]
        public int IdEstadoUsuario { get; set; }

        /// <summary>
        /// Si existe algún conflicto por nro. de documento repetido en casos de:
        ///   .- Tipo de Documento
        ///   .- País
        /// </summary>
        [JsonProperty(PropertyName = "conflicto")]
        public bool Conflicto { get; set; }

        /// <summary>
        /// TiposDocumento
        /// </summary>
        [JsonProperty(PropertyName = "tipos_documento")]
        public List<TiposDocumentoModelResponse> TiposDocumento { get; set; }

        /// <summary>
        /// Paises
        /// </summary>
        [JsonProperty(PropertyName = "paises")]
        public List<PaisesModelResponse> Paises { get; set; }

        /// <summary>
        /// Constructor ValidacionExistenciaModelResponse
        /// </summary>
        protected ValidacionExistenciaModelResponse()
        {
            /* Empty constructor */
        }

        private static ValidacionExistenciaModelResponse FromGetValidacionExistencia(ValidacionExistenciaModelOutput output)
        {
            return FromValidacionExistenciaModelOutput(output);
        }

        private static ValidacionExistenciaModelResponse FromValidacionExistenciaModelOutput(ValidacionExistenciaModelOutput output)
        {
            var tiposDocumento = new List<TiposDocumentoModelResponse>();
            if (output.TiposDocumento?.Count > 1)
            {
                tiposDocumento = (List<TiposDocumentoModelResponse>)TiposDocumentoModelResponse.FromTiposDocumentoModelOutput(
                    output.TiposDocumento);
            }

            var paises = new List<PaisesModelResponse>();
            if (output.Paises?.Count > 1)
            {
                paises = (List<PaisesModelResponse>)PaisesModelResponse.FromPaisesModelOutput(
                    output.Paises);
            }

            return new ValidacionExistenciaModelResponse
            {
                Telefono = output.Telefono ?? string.Empty,
                Migrado = output.Migrado,
                IdPersona = output.IdPersona,
                Usuario = string.Empty, // Se evita enviar el Usuario en este servicio por temas de seguridad CDIG-2894
                IdEstadoUsuario = output.IdEstadoUsuario,
                Conflicto = tiposDocumento.Count > 0 || paises.Count > 0,
                TiposDocumento = tiposDocumento,
                Paises = paises
            };
        }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<ValidacionExistenciaModelResponse>> FromAsync(Task<IResponse<ValidacionExistenciaModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetValidacionExistencia);
        }

    }
}
