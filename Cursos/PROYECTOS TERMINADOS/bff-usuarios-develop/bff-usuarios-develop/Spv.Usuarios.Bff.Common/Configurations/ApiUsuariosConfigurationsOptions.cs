namespace Spv.Usuarios.Bff.Common.Configurations
{
    public class ApiUsuariosConfigurationsOptions
    {
        public string BasePath { get; set; }

        public string PerfilPath { get; set; }

        public string PerfilPathV2 { get; set; }

        public string ValidarClaveCanalesPath { get; set; }

        public string InhabilitarClaveCanalesPath { get; set; }

        public string RegistrarUsuarioPath { get; set; }

        public string ValidarExistenciaPath { get; set; }

        public string ValidarExistenciaHbiPath { get; set; }

        public string CambiarCredencialesPath { get; set; }

        public string CambiarClavePath { get; set; }

        public string MigrarUsuarioPath { get; set; }

        public string ActualizarPersonIdPath { get; set; }

        public string AutenticacionPath { get; set; }

        public string ObtenerUsuarioPath { get; set; }

        public int CacheExpiracionTyCMinutos { get; set; }

        public int CacheExpiracionImagenesLoginMinutos { get; set; }

        public string ObtenerImagenesLoginPath { get; set; }

        public string ObtenerEstadoClaveCanalesPath { get; set; }
    }
}
