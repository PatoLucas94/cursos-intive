using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spv.Usuarios.Bff.Common.Constants;
using Spv.Usuarios.Bff.Common.Dtos.Client.PersonasClient.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.CatalogoService.Output;
using Spv.Usuarios.Bff.Common.Dtos.Service.PersonaService.Output;
using Spv.Usuarios.Bff.DataAccess.ExternalWebService.Interfaces;
using Spv.Usuarios.Bff.Domain.Exceptions;

namespace Spv.Usuarios.Bff.Service.Helpers
{
    public static class PersonasHelper
    {
        /// <summary>
        /// Obtener información completa sobre una determinada Persona Física
        /// </summary>
        /// <param name="personasRepository">Inyección de dependencia por parámetros</param>
        /// <param name="personaId">Id Persona</param>
        /// <returns></returns>
        public static async Task<PersonaModelOutput> GetInfoPersonaFisicaAsync(IApiPersonasRepository personasRepository, int personaId)
        {
            // Obtenemos Persona Física
            var personaFisica = await personasRepository.ObtenerInfoPersonaFisicaAsync(personaId.ToString());
            if (personaFisica == null)
            {
                throw new BusinessException(MessageConstants.PersonaFisicaInexistente(personaId), 0);
            }

            // Obtenemos el teléfono doble factor desde los datos de la persona
            var telefono = personaFisica.telefonos?.FirstOrDefault(t =>
                                    (t.doble_factor ?? false)
                                    && (!t.dado_de_baja ?? false));

            telefono ??= personaFisica.telefonos?.OrderByDescending(t => t.fecha_creacion)
                                    .FirstOrDefault(t => (!t.dado_de_baja ?? false)
                                        && t.tipo_telefono.ToUpper().Trim() == AppConstants.TipoTelefonoCelular);

            // Obtenemos Email Principal
            var emailConfiable = personaFisica.emails?.FirstOrDefault(e => e.principal == true);

            var result = new PersonaModelOutput
            {
                Id = personaFisica.id,
                PaisDocumento = personaFisica.pais_documento,
                TipoDocumento = personaFisica.tipo_documento,
                NumeroDocumento = personaFisica.numero_documento,
                Email = emailConfiable?.direccion,
                Telefono = telefono?.numero,
                TelefonoDobleFactor = telefono?.doble_factor ?? false,
                TipoPersona = personaFisica.tipo_persona,
                Nombre = personaFisica.nombre,
                Apellido = personaFisica.apellido,
                Genero = personaFisica.genero,
                FechaNacimiento = personaFisica.fecha_nacimiento,
                PaisNacimiento = personaFisica.pais_nacimiento,
            };

            return result;
        }

        public static bool ExisteConflictoConDniVsLibretas(
            string numeroDocumento, 
            IReadOnlyCollection<ApiPersonasFiltroModelOutput> personas)
        {
            return long.TryParse(numeroDocumento, out var nroDoc) && nroDoc < 10000000 &&
                   personas.Any(dni => dni.tipo_documento == (int)TipoDocumento.Dni) &&
                   (personas.Any(le => le.tipo_documento == (int)TipoDocumento.Le)
                    || personas.Any(lc => lc.tipo_documento == (int)TipoDocumento.Lc));
        }

        public static async Task<ConflictoModelOutput> RecopilarConflictosDePersonas(
            IApiCatalogoRepository apiCatalogoRepository,
            List<TipoDocumentoModelOutput> tiposDocumento,
            List<PaisModelOutput> paises)
        {
            var conflicto = new ConflictoModelOutput();
            var codTipoDocumentoMuestra = tiposDocumento.First().codigo;
            var codPaisMuestra = paises.First().codigo;

            if (tiposDocumento.Any(x => x.codigo != codTipoDocumentoMuestra))
            {
                conflicto.TiposDocumento = tiposDocumento;
            }

            if (paises.Any(x => x.codigo != codPaisMuestra))
            {
                var paisesCatalogo = await apiCatalogoRepository.ObtenerPaisesAsync();

                paises.ForEach(p =>
                    p.descripcion = paisesCatalogo
                        .FirstOrDefault(pc => pc.codigo == p.codigo)?.descripcion);

                conflicto.Paises = paises;
            }

            return conflicto;
        }
    }
}
