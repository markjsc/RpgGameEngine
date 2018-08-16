using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    /// <summary>
    /// A class to facilitate navigating the Game State.
    /// This is primarily to be used internally by the GameContext
    /// although it could also be used by other consumers, if needed.
    /// </summary>
    public interface IGameStateNavigator
    {
        /// <summary>
        /// Gets an Element from its Id
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        GameElement GetElementById(List<GameElement> elements, string id);

        /// <summary>
        /// Gets an Action by its Id
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        GameAction GetActionById(List<GameElement> elements, string actionId);

        /// <summary>
        /// Gets a list of Triggers by Target.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        List<GameTrigger> GetTriggersByTarget(List<GameElement> elements, string targetId);
    } 
}
