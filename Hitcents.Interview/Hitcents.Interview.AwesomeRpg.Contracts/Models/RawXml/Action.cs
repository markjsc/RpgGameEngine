using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    public class Action
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Setter")]
        public Setter Setter { get; set; }
    }
}
