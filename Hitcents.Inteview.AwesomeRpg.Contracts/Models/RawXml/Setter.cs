using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    public class Setter
    {
        [XmlAttribute(AttributeName = "Target")]
        public string Target { get; set; }

        [XmlAttribute(AttributeName = "Operation")]
        public string Operation { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }
}
