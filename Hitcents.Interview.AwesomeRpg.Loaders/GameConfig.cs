using System.Xml.Serialization;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;

namespace Hitcents.Interview.AwesomeRpg.Loaders
{
    /// <summary>
    /// This is the root element. Expected child is Elements (Array of Element).
    /// This class is ONLY used by the XML Loader to wrap the XML at load-time.
    /// This is a dependency of the XML Loader class and is needed in THIS project
    /// rather than in the Contracts.
    /// </summary>
    [XmlRoot(ElementName = "GameConfig")]
    public class GameConfig
    {
        [XmlArray(ElementName = "Elements")]
        public Element[] Elements { get; set; }
    }
}
