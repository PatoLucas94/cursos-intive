using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Domain.ApiEntities
{
    public sealed class Cuil : Id<Cuil>
    {
        public const long ValorMinimo = 20000000000;
        public const long ValorMaximo = 34999999999;

        public Cuil(long valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<Cuil> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new Cuil(i));
        }
    }
}
