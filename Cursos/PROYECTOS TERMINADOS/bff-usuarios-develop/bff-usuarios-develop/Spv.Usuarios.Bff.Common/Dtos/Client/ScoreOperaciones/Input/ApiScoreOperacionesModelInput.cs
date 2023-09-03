namespace Spv.Usuarios.Bff.Common.Dtos.Client.ScoreOperaciones.Input
{
    public class ApiScoreOperacionesModelInput
    {
        public string IdPersona { get; set; }
        public string CBU { get; set; }
        public string IdDispositivo { get; set; }
        public string AccionDelEvento { get; set; }
        public string Motivo { get; set; }
        public string TipoDeAccion { get; set; }
        public string NumeroReferencia { get; set; }
        public string ActualizarEntidad { get; set; }
    }
}
