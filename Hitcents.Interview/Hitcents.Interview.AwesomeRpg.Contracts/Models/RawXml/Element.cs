using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    public class Element
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }

        [XmlElement(ElementName = "Element")]
        public Element[] Elements { get; set; }

        [XmlElement(ElementName = "Action")]
        public Action[] Actions { get; set; }
        
        [XmlElement(ElementName = "Trigger")]
        public Trigger[] Triggers { get; set; }
    }
}
