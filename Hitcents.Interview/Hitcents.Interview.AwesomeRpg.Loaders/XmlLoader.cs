using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hitcents.Interview.AwesomeRpg.Loaders
{
    public class XmlLoader : IXmlLoader
    {
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
