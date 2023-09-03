using System.Collections.Generic;

namespace Spv.Usuarios.Common.Dtos.UsuariosService.Output
{
    public class ObtenerPinModelOutput
    {
        public BtinreqOutput Btinreq { get; set; }
        public DatosPin DatosPIN { get; set; }
        public Erroresnegocio Erroresnegocio { get; set; }
    }

    public class BtinreqOutput
    {
        public string Canal { get; set; }
        public string Token { get; set; }
        public string Usuario { get; set; }
        public string Requerimiento { get; set; }
        public string Device { get; set; }
    }

    public class DatosPin
    {
        public string pin { get; set; }
        public int tipdoc { get; set; }
        public string esTemporal { get; set; }
        public string ulting { get; set; }
        public int pais { get; set; }
        public string existe { get; set; }
        public string fvto { get; set; }
        public int intento { get; set; }
        public string numdoc { get; set; }
    }

    public class Erroresnegocio
    {
        public List<BTErrorNegocio> BTErrorNegocio { get; set; }
    }

    public class BTErrorNegocio
    {
        public string Severidad { get; set; }
        public string Descripcion { get; set; }
        public int Codigo { get; set; }
    }
}
