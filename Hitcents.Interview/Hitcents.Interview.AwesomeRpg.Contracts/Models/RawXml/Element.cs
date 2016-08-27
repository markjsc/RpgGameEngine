using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    public class Element
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Value - int, for now (possibly a string later?)
        /// </summary>
        [XmlAttribute(AttributeName = "Value")]
        public int Value { get; set; }

        [XmlElement(ElementName = "Element")]
        public Element[] Elements { get; set; }

        [XmlElement(ElementName = "Action")]
        public Action Action { get; set; }
        
        [XmlElement(ElementName = "Trigger")]
        public Trigger Trigger { get; set; }
    }
}
