using System;
using System.Collections.Generic;
using System.IO;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;

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
        /// This is just basic, plain-ol' Deserializin' - with ONE exception:
        /// - In an effort to not require the user to provide the two outer tags (document root and elements grouping),
        /// there's some string manipulation here to add the extra tags required by the deserializer.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<Element> LoadXml(string xml)
        {
            try
            {
                var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(GameConfig));

                //HACK: In order to prevent the user from having to provide the outer two tags required to properly deserialize
                // (<GameConfig><Elements>...</Elements></GameConfig>) they are added here using basic string manipulation. 
                // In a larger system, this would be done more elegantly!
                // (This is actually based on an assumption from the literal XML provided in ReadMe.MD. 
                // My first iteration required the XML to have these tags and did NOT include this hack!
                // If there's a possibility of having the incoming XML include additional surrounding tags then this gets simpler.)
                
                //I should probably use Reflection to pull in the XmlRoot.ElementName from the GameConfig and Element properties...
                var completeXmlWithHackyOuterTags = string.Format("<GameConfig><Elements>{0}</Elements></GameConfig>", xml);

                var root = (GameConfig)deserializer.Deserialize(new StringReader(completeXmlWithHackyOuterTags));
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
