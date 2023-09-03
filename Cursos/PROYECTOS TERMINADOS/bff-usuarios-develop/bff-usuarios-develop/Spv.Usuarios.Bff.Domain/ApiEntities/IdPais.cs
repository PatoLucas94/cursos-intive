using Spv.Usuarios.Bff.Domain.Utils;

namespace Spv.Usuarios.Bff.Domain.ApiEntities
{
    public sealed class IdPais : Id<IdPais>
    {
        public const int ValorMinimo = 1;
        public const int ValorMaximo = int.MaxValue;

        public IdPais(int valor) : base(Arg.InRange(valor, ValorMinimo, ValorMaximo, nameof(valor)))
        {
        }

        public static Result<IdPais> TryParse(string s, string context = null)
        {
            return TryParseValue(s, ValorMinimo, ValorMaximo, context)
                .Map(i => new IdPais((int)i));
        }
    }
}
