using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    public interface IGameStateLoader
    {
        List<GameElement> LoadGameState(List<Element> elements);
    }
}
