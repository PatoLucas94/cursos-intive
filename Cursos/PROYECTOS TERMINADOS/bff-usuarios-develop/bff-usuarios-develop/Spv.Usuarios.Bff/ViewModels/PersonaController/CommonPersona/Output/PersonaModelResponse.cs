using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.Domain.Services;
using Spv.Usuarios.Bff.ViewModels.CatalogoController.CommonCatalogo.Output;

namespace Spv.Usuarios.Bff.ViewModels.PersonaController.CommonPersona.Output
{
    /// <summary>
    /// PersonaModelResponse
    /// </summary>
    [JsonObject(Title = "persona")]
    public class PersonaModelResponse
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// PaisDocumento
        /// </summary>
        [JsonProperty(PropertyName = "pais_documento")]
        public int PaisDocumento { get; set; }

        /// <summary>
        /// TipoDocumento
        /// </summary>
        [JsonProperty(PropertyName = "tipo_documento")]
        public int TipoDocumento { get; set; }

        /// <summary>
        /// NumeroDocumento
        /// </summary>
        [JsonProperty(PropertyName = "numero_documento")]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Telefono
        /// </summary>
        [JsonProperty(PropertyName = "telefono")]
        public string Telefono { get; set; }

        /// <summary>
        /// tipo_persona
        /// </summary>
        [JsonProperty(PropertyName = "tipo_persona")]
        public string TipoPersona { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [JsonProperty(PropertyName = "apellido")]
        public string Apellido { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        [JsonProperty(PropertyName = "genero")]
        public string Genero { get; set; }

        /// <summary>
        /// FechaNacimiento
        /// </summary>
        [JsonProperty(PropertyName = "fecha_nacimiento")]
        public string FechaNacimiento { get; set; }

        /// <summary>
        /// PaisNacimiento
        /// </summary>
        [JsonProperty(PropertyName = "pais_nacimiento")]
        public int? PaisNacimiento { get; set; }

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
        /// Constructor PersonaModelResponse
        /// </summary>
        protected PersonaModelResponse()
        {
            /* empty constructor */
        }

        private static PersonaModelResponse FromGetPersona(PersonaModelOutput output)
        {
            return FromPersonaModelOutput(output);
        }

        private static PersonaModelResponse FromPersonaModelOutput(PersonaModelOutput output)
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

            return new PersonaModelResponse
            {
                Id = output.Id,
                PaisDocumento = output.PaisDocumento,
                TipoDocumento = output.TipoDocumento,
                NumeroDocumento = output.NumeroDocumento,
                Email = output.Email,
                Telefono = output.Telefono,
                TipoPersona = output.TipoPersona,
                Nombre = output.Nombre,
                Apellido = output.Apellido,
                Genero = output.Genero,
                FechaNacimiento = output.FechaNacimiento,
                PaisNacimiento = output.PaisNacimiento,
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
        public static async Task<IResponse<PersonaModelResponse>> FromAsync(Task<IResponse<PersonaModelOutput>> task)
        {
            var response = await task;
            return response.Map(FromGetPersona);
        }

    }
}
