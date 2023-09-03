using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Common.Information
{
    public class InformationCode
    {
        public string Code { get; }
        public string InformationDescription { get; }

        public InformationCode(string code, string infoDescription)
        {
            Code = Arg.NonNullNorSpaces(code, nameof(code));
            InformationDescription = Arg.NonNullNorSpaces(infoDescription, nameof(infoDescription));
        }

        public override bool Equals(object obj) => obj is InformationCode other
                                           && Equals(Code, other.Code)
                                           && Equals(InformationDescription, other.InformationDescription);

        public override int GetHashCode()
        {
            return Code.GetHashCode() ^ InformationDescription.GetHashCode();
        }

        public static InformationCode ClaveActualVacia => new InformationCode(InfomationConstants.CodigoPasswordVacia, MessageConstants.ClaveActualVacia);
    }
}
