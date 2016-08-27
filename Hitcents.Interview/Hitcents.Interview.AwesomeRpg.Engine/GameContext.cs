using System;
using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Engine
{
    public class GameContext : IGameContext
    {
        private List<GameElement> _gameState;
        private Dictionary<string, GameElement> _flattenedElements;


        public GameContext()
        {
            this._flattenedElements = new Dictionary<string, GameElement>();
        }

        public void BuildGameContext(List<GameElement> gameState)
        {
            try
            {
                this._gameState = gameState;
                this.RegisterElementsById(gameState);
            }
            catch(Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception("Buliding the Game Context has failed.", ex);
            }
        }

        public void RunAction(string actionId)
        {
            try
            {

            }
            catch(Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("Running the action for Action with Id {0} has failed.", actionId), ex);
            }
        }

        private void RegisterElementsById(List<GameElement> elements)
        {
            foreach(var element in elements)
            {
                if(!this._flattenedElements.ContainsKey(element.Id))
                {
                    this._flattenedElements.Add(element.Id, element);
                }
                else
                {
                    throw new Exception(string.Format("Failed to register Element with Id {0} - it has already been registered. Check for duplicate Ids.", element.Id));
                }

                //Make recursive call to register children
                this.RegisterElementsById(element.Elements);
            }
        }
    }
}
