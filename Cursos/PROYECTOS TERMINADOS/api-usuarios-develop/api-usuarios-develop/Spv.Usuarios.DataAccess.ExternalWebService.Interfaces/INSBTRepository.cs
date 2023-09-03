using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.NSBTClient;

namespace Spv.Usuarios.DataAccess.ExternalWebService.Interfaces
{
    public interface INsbtRepository
    {
        Task<PinFromNsbt> GetPinAsync(string countryId, int documentTypeId, string documentNumber);
        Task IncrementLoginAttemptsAsync(
            string countryId, 
            int documentTypeId, 
            string documentNumber, 
            string pin, 
            int attempt, 
            string preservedDateTime = null);
    }
}
