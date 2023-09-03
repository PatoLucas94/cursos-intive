using System.Diagnostics.CodeAnalysis;
using Spv.Usuarios.Domain.Enums;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Input
{
    [ExcludeFromCodeCoverage]
    public class CambioEstadoModelInput
    {
        public long PersonId { get; set; }

        public UserStatus EstadoId { get; set; }
    }
}