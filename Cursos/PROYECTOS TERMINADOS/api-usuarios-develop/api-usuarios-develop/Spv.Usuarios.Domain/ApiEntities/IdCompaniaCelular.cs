using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class IdCompaniaCelular : Id<IdCompaniaCelular>
    {
        public const int ValorMinimo = 0;
        public const int ValorMaximo = int.MaxValue;

        public IdCompaniaCelular(int valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<IdCompaniaCelular> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new IdCompaniaCelular((int)i));
        }
    }
}
