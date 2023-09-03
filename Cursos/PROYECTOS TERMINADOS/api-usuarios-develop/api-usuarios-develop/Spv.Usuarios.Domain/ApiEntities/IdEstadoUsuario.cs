using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class IdEstadoUsuario : Id<IdEstadoUsuario>
    {
        public const int ValorMinimo = 0;
        public const int ValorMaximo = 255;

        public IdEstadoUsuario(int valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<IdEstadoUsuario> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new IdEstadoUsuario((int)i));
        }
    }
}
