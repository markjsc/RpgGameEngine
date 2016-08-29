using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    public interface IGameStateNavigator
    {
        GameElement GetElementById(List<GameElement> elements, string id);

        GameAction GetActionById(List<GameElement> elements, string actionId);

        List<GameTrigger> GetTriggersByTarget(List<GameElement> elements, string targetId);
    } 
}
