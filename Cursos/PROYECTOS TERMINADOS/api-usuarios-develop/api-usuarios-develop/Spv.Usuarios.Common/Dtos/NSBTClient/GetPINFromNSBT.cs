using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Spv.Usuarios.Common.Dtos.NSBTClient
{
    [XmlRoot(ElementName = "SdtPIN", Namespace = "KB_80INTERFASES")]
    public class PinFromNsbt : IXmlSerializable
    {
        [XmlElement("Pais", Namespace = "KB_80INTERFASES")]
        public string CountryId { get; set; }

        [XmlElement("tipdoc", Namespace = "KB_80INTERFASES")]
        public int DocumentTypeId { get; set; }

        [XmlElement("numdoc", Namespace = "KB_80INTERFASES")]
        public string DocumentNumber { get; set; }

        [XmlElement("pin", Namespace = "KB_80INTERFASES")]
        public string Pin { get; set; }

        [XmlElement("intento", Namespace = "KB_80INTERFASES")]
        public int Attempt { get; set; }

        [XmlElement("ulting", Namespace = "KB_80INTERFASES")]
        public string LastLogIn { get; set; }

        [XmlElement("Fvto", Namespace = "KB_80INTERFASES")]
        public DateTime ExpirationDate { get; set; }

        [XmlElement("Existe", Namespace = "KB_80INTERFASES")]
        public bool Exists { get; set; }

        [XmlElement("EsTemporal", Namespace = "KB_80INTERFASES")]
        public string IsTemporal { get; set; }

        private readonly string[] _xmlValues =
        {
            "Pais", 
            "tipdoc", 
            "numdoc", 
            "pin", 
            "intento", 
            "ulting", 
            "Fvto", 
            "Existe", 
            "EsTemporal"
        };

        public XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();

            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element || _xmlValues.Contains(reader.Name))
                {
                    XElement el = XNode.ReadFrom(reader) as XElement;
                    FillValue(el?.Name.LocalName, el?.Value);
                }
                else reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        private void FillValue(string name, string value)
        {
            switch (name)
            {
                case "pin":
                    Pin = value;
                    break;
                case "Fvto":
                    ExpirationDate = value == "0000-00-00" ? DateTime.Now : DateTime.Parse(value);
                    break;
                case "EsTemporal":
                    IsTemporal = value;
                    break;
                case "Existe":
                    Exists = value == "1";
                    break;
                case "Pais":
                    CountryId = value;
                    break;
                case "tipdoc":
                    DocumentTypeId = int.Parse(value);
                    break;
                case "numdoc":
                    DocumentNumber = value;
                    break;
                case "intento":
                    Attempt = int.Parse(value);
                    break;
                case "ulting":
                    LastLogIn = value;
                    break;
            }
        }
    }
}
