namespace Spv.Usuarios.Bff.Common.Dtos.Client.UsuariosClient.Output
{
    /// <summary>
    /// ApiUsuariosImagenLoginModelOutput
    /// </summary>
    public class ApiUsuariosImagenLoginModelOutput
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int IdImagen { get; set; }

        public string Link { get; set; }

        public int Orden { get; set; }

        public bool Habilitada { get; set; }
    }
}
