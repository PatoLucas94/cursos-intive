using System;
using System.Net;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.TyCClient.Output
{
    public class ApiTyCAceptadosModelOutput
    {
        public string id_terminos_condiciones { get; set; }

        public DateTime vigencia_desde { get; set; }

        public DateTime fecha_aceptacion { get; set; }

        public HttpStatusCode status_code { get; set; }
    }
}
