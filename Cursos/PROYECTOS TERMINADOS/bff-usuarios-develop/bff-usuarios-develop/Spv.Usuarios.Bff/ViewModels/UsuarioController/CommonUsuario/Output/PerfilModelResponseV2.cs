using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Service.UsuarioService.Output;
using Spv.Usuarios.Bff.Domain.Services;

namespace Spv.Usuarios.Bff.ViewModels.UsuarioController.CommonUsuario.Output
{
    /// <summary>
    /// PerfilModelResponseV2
    /// </summary>
    [JsonObject(Title = "usuario")]
    public class PerfilModelResponseV2
    {
        /// <summary>
        /// Fecha último login
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.UltimoLogin)]
        public DateTime? UltimoLogin { get; set; }

        /// <summary>
        /// Id persona
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdPersona)]
        public long? IdPersona { get; set; }

        /// <summary>
        /// Id usuario
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.IdUsuario)]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.NroDocumento)]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Email)]
        public string Email { get; set; }

        /// <summary>
        /// Nombre 
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Nombre)]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Apellido)]
        public string Apellido { get; set; }

        /// <summary>
        /// TipoDocumento
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.TipoDocumento)]
        public int TipoDocumento { get; set; }

        /// <summary>
        /// Pais
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Pais)]
        public int Pais { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.Genero)]
        public string Genero { get; set; }

        /// <summary>
        /// Fecha Ultimo Cambio Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.FechaUltimoCambioClave)]
        public DateTime? FechaUltimoCambioClave { get; set; }

        /// <summary>
        /// Fecha Vencimiento Clave
        /// </summary>
        [JsonProperty(PropertyName = ParameterNames.FechaVencimientoClave)]
        public DateTime? FechaVencimientoClave { get; set; }

        /// <summary>
        /// FromAsync
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<IResponse<PerfilModelResponseV2>> FromAsync(Task<IResponse<PerfilModelOutputV2>> task)
        {
            var response = await task;
            return response.Map(FromGetPerfil);
        }

        private static PerfilModelResponseV2 FromGetPerfil(PerfilModelOutputV2 output)
        {
            return FromProfileModelOutput(output);
        }

        private static PerfilModelResponseV2 FromProfileModelOutput(PerfilModelOutputV2 output)
        {
            return new PerfilModelResponseV2
            {
                UltimoLogin = output.UltimoLogin,
                IdPersona = output.IdPersona,
                IdUsuario = output.IdUsuario,
                NumeroDocumento = output.NumeroDocumento,
                Email = output.Email,
                Nombre = output.Nombre,
                Apellido = output.Apellido,
                TipoDocumento = output.TipoDocumento,
                Genero = output.Genero,
                Pais = output.Pais,
                FechaUltimoCambioClave = output.FechaUltimoCambioClave,
                FechaVencimientoClave = output.FechaVencimientoClave
            };
        }
    }
}
