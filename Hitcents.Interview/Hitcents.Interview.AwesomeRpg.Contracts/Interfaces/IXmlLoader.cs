using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    public interface IXmlLoader
    {
        List<Element> LoadXml(string xml);
    }
}
