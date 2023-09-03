using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class IdPersona : Id<IdPersona>
    {
        public const long ValorMinimo = 1;
        public const long ValorMaximo = long.MaxValue;

        public IdPersona(long valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<IdPersona> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new IdPersona(i));
        }
    }
}
