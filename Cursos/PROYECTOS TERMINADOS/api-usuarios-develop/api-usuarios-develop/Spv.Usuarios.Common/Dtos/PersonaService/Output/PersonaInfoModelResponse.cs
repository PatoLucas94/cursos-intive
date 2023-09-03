using System.Collections.Generic;

namespace Spv.Usuarios.Common.Dtos.PersonaService.Output
{
    public class ActividadEconomicaAfip
    {
        public int? codigo_actividad { get; set; }
        public string descripcion_actividad { get; set; }
        public string fecha_actualizacion { get; set; }
        public int? id { get; set; }
        public int? orden { get; set; }
    }

    public class DeclaracionFatca
    {
        public int? categoria { get; set; }
        public string numero { get; set; }
    }

    public class DeclaracionOcde
    {
        public bool? declara_ocde { get; set; }
        public string fecha_declaracion { get; set; }
        public string identificador_unico_ocde { get; set; }
    }

    public class DeclaracionUif
    {
        public bool? es_sujeto_obligado_uif { get; set; }
        public int? tipo_sujeto_obligado_uif { get; set; }
    }

    public class DocumentoAdicional
    {
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public string numero_documento { get; set; }
        public int? pais_documento { get; set; }
        public int? tipo_documento { get; set; }
    }

    public class Domicilio
    {
        public string calle { get; set; }
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public int? codigo_postal { get; set; }
        public string codigo_postal_argentino { get; set; }
        public string departamento { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public string latitud { get; set; }
        public string legal { get; set; }
        public int? localidad { get; set; }
        public string localidad_alfanumerica { get; set; }
        public string localidad_maestro { get; set; }
        public string longitud { get; set; }
        public bool? normalizado { get; set; }
        public string numero { get; set; }
        public string origen_contacto { get; set; }
        public int? pais { get; set; }
        public string piso { get; set; }
        public int? provincia { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
    }

    public class EstadoPersonaExpuestaPoliticamente
    {
        public bool? esta_expuesta { get; set; }
        public string motivo { get; set; }
    }

    public class Etiqueta
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Email
    {
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string cargo_interlocutor { get; set; }
        public bool? confiable { get; set; }
        public bool? dado_de_baja { get; set; }
        public string direccion { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public string nombre_interlocutor { get; set; }
        public string origen_contacto { get; set; }
        public bool? principal { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
    }

    public class Impuesto
    {
        public int? condicion { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public int? tipo_impuesto { get; set; }
    }

    public class Telefono
    {
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string cargo_interlocutor { get; set; }
        public int? codigo_area { get; set; }
        public string compania { get; set; }
        public bool? confiable { get; set; }
        public bool? dado_de_baja { get; set; }
        public string ddi { get; set; }
        public string ddn { get; set; }
        public bool? doble_factor { get; set; }
        public bool? es_geografico { get; set; }
        public List<Etiqueta> etiquetas { get; set; }
        public string fecha_alta_no_llame { get; set; }
        public string fecha_baja_no_llame { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public int? interno { get; set; }
        public string no_llame { get; set; }
        public string nombre_interlocutor { get; set; }
        public bool? normalizado { get; set; }
        public string numero { get; set; }
        public int? numero_local { get; set; }
        public string origen_contacto { get; set; }
        public int? pais { get; set; }
        public int? prefijo_telefonico_pais { get; set; }
        public bool? principal { get; set; }
        public decimal? score { get; set; }
        public string tipo_telefono { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
        public UltimaVerificacionPositiva ultima_verificacion_positiva { get; set; }
    }

    public class UltimaVerificacionPositiva
    {
        public string canal_verificacion { get; set; }
        public string fecha_verificacion { get; set; }
        public string usuario_verificacion { get; set; }
    }

    public class TokenPush
    {
        public string canal_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public string sistema_operativo { get; set; }
        public string token { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
    }

    public class VinculoPersona
    {
        public string canal_creacion { get; set; }
        public string fecha_creacion { get; set; }
        public int? id { get; set; }
        public int? id_persona_fisica { get; set; }
        public int? id_persona_fisica_vinculada { get; set; }
        public string usuario_creacion { get; set; }
        public int? vinculo { get; set; }
    }

    public class PersonaInfoModelResponse
    {
        public List<ActividadEconomicaAfip> actividades_economicas_afip { get; set; }
        public string canal_creacion { get; set; }
        public int? canal_distribucion { get; set; }
        public string canal_modificacion { get; set; }
        public string categoria { get; set; }
        public DeclaracionFatca declaracion_fatca { get; set; }
        public DeclaracionOcde declaracion_ocde { get; set; }
        public DeclaracionUif declaracion_uif { get; set; }
        public List<DocumentoAdicional> documentos_adicionales { get; set; }
        public List<Domicilio> domicilios { get; set; }
        public List<Email> emails { get; set; }
        public string estado_validacion_documento { get; set; }
        public string fecha_alta_bt { get; set; }
        public string fecha_baja_bt { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public int? id { get; set; }
        public List<Impuesto> impuestos { get; set; }
        public string numero_documento { get; set; }
        public string numero_tributario { get; set; }
        public int? pais_documento { get; set; }
        public int? pais_residencia { get; set; }
        public List<Telefono> telefonos { get; set; }
        public int? tipo_documento { get; set; }
        public string tipo_persona { get; set; }
        public int? tipo_tributario { get; set; }
        public List<TokenPush> tokens_push { get; set; }
        public string usuario_creacion { get; set; }
        public string usuario_modificacion { get; set; }
    }

    public class PersonaFisicaInfoModelResponse
    {
        public int id { get; set; }
        public int pais_documento { get; set; }
        public int tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public List<Email> emails { get; set; }
        public List<TokenPush> tokens_push { get; set; }
        public List<Telefono> telefonos { get; set; }
        public List<Domicilio> domicilios { get; set; }
        public List<DocumentoAdicional> documentos_adicionales { get; set; }
        public List<ActividadEconomicaAfip> actividades_economicas_afip { get; set; }
        public List<Impuesto> impuestos { get; set; }
        public string tipo_persona { get; set; }
        public string fecha_alta_bt { get; set; }
        public string fecha_baja_bt { get; set; }
        public string categoria { get; set; }
        public int? canal_distribucion { get; set; }
        public string canal_creacion { get; set; }
        public string usuario_creacion { get; set; }
        public string canal_modificacion { get; set; }
        public string usuario_modificacion { get; set; }
        public int? tipo_tributario { get; set; }
        public string numero_tributario { get; set; }
        public DeclaracionOcde declaracion_ocde { get; set; }
        public DeclaracionFatca declaracion_fatca { get; set; }
        public DeclaracionUif declaracion_uif { get; set; }
        public string estado_validacion_documento { get; set; }
        public int? pais_residencia { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string genero { get; set; }
        public string fecha_nacimiento { get; set; }
        public int? estado_civil { get; set; }
        public int? paquete { get; set; }
        public int? pais_nacimiento { get; set; }
        public bool? es_ciudadano_legal { get; set; }
        public bool? marca_acredita_sueldo { get; set; }
        public bool? es_empleado { get; set; }
        public int? registro_patrimonial_matrimonio { get; set; }
        public string lugar_nacimiento { get; set; }
        public List<VinculoPersona> vinculos_de_personas { get; set; }
        public EstadoPersonaExpuestaPoliticamente estado_persona_expuesta_politicamente { get; set; }
        public bool? fallecido { get; set; }
        public string fecha_de_fallecimiento { get; set; }
        public string fecha_de_consulta_renaper { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_modificacion { get; set; }
        public bool? es_titular { get; set; }
    }
}
