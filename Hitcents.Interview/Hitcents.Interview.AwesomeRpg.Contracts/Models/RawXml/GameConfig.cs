using System.Xml.Serialization;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml
{
    /// <summary>
    /// This is the root element. Expected child is Elements (Array of Element).
    /// </summary>
    [XmlRoot(ElementName = "GameConfig")]
    public class GameConfig
    {
        [XmlArray(ElementName = "Elements")]
        public Element[] Elements { get; set; }
    }
}
