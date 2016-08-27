using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    public interface IGameContext
    {
        void BuildGameContext(List<GameElement> gameState);

        void RunAction(string actionId);
    }
}
