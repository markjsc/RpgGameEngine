using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Interfaces
{
    /// <summary>
    /// GameContext holds the Game State and facilitates running actions.
    /// </summary>
    public interface IGameContext
    {
        /// <summary>
        /// Builds the Game Context from the provided list of GameElements.
        /// </summary>
        /// <param name="gameState"></param>
        void BuildGameContext(List<GameElement> gameState);

        /// <summary>
        /// Runs the Action with the matching Id. This will also handle
        /// firing any Triggers (recursively).
        /// </summary>
        /// <param name="actionId"></param>
        void RunAction(string actionId);

        /// <summary>
        /// A read-only collection of the Game State, which could be
        /// used to display in a non-editable UI.
        /// </summary>
        IReadOnlyCollection<GameElement> GameState { get; }
    }
}
