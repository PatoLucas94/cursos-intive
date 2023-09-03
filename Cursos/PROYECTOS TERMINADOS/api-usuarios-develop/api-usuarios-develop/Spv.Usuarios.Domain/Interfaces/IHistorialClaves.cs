using System;

namespace Spv.Usuarios.Domain.Interfaces
{
    public interface IHistorialClaves
    {
        int GetUserId();
        string GetPassword();
        DateTime GetCreationDate();
    }
}
