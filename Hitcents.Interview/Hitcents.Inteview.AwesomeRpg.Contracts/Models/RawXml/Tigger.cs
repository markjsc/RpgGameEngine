using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    public class Trigger
    {
        [XmlAttribute(AttributeName = "Target")]
        public string Target { get; set; }

        [XmlAttribute(AttributeName = "Comparison")]
        public string Comparison { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }

        [XmlElement(ElementName = "Setter")]
        public Setter[] Setters { get; set; }
    }
}
