using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    /// <summary>
    /// A class to facilitate loading the data objects and returning the domain objects.
    /// </summary>
    public interface IGameStateLoader
    {
        /// <summary>
        /// Returns a list of GameElements (domain objects) from a list
        /// of Elements (data/xml objects).
        /// Performs some basic validation and transformation.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        List<GameElement> LoadGameState(List<Element> elements);
    }
}
