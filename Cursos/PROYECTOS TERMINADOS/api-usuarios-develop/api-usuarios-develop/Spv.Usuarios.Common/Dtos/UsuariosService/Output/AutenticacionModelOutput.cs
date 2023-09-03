using Spv.Usuarios.Common.Errors;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    public class AutenticacionModelOutput
    {
        public long? IdPersona { get; set; }
        public Codigo Codigo { get; set; }
        public ErrorCode Error { get; set; }
    }

    public enum Codigo
    {
        Incorrecto = 0,
        Aceptado = 3,
        Bloqueado = 5,
        Inactivo = 7,
        Suspendido = 9,
        EstadoDeUsuarioNoControlado = 10
    }
}
