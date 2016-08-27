using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hitcents.Interview.AwesomeRpg.Loaders
{
    /// <summary>
    /// This class is responsible for loading the XML ONLY.
    /// </summary>
    public class XmlLoader : IXmlLoader
    {
        /// <summary>
        /// This method populates the Raw XML objects from the provided XML.
        /// There is no conversion or business logic performed here.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<Element> LoadXml(string xml)
        {
            try
            {
                var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(GameConfig));
                var root = (GameConfig)deserializer.Deserialize(new StringReader(xml));
                return new List<Element>(root.Elements);
            }
            catch(Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("An exception occurred while loading the XML.", ex));
            }
        }
    }
}
