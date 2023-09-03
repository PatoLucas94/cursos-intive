namespace Spv.Usuarios.Api.Helpers
{
    /// <summary>
    /// IAllowedChannels
    /// </summary>
    public interface IAllowedChannels
    {
        /// <summary>
        /// Método que retorna 'true' si el Header X-Canal es valido
        /// </summary>
        bool IsValidChannel(string pChannel);
    }
}
