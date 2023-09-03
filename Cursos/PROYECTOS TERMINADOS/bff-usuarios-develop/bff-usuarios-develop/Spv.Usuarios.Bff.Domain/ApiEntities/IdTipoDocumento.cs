using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class IdTipoDocumento : Id<IdTipoDocumento>
    {
        public const int ValorMinimo = 1;
        public const int ValorMaximo = byte.MaxValue;

        public IdTipoDocumento(int valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<IdTipoDocumento> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new IdTipoDocumento((int)i));
        }
    }
}
