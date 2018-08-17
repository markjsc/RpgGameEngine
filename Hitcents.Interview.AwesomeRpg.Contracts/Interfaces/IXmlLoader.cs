using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    /// <summary>
    /// A class used to load XML into data/xml objects.
    /// </summary>
    public interface IXmlLoader
    {
        /// <summary>
        /// Returns a list of Element data/xml objects from a string of XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        List<Element> LoadXml(string xml);
    }
}
