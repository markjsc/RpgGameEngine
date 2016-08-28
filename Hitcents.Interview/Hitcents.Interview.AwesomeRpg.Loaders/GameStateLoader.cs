using System;
using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;

namespace Hitcents.Interview.AwesomeRpg.Loaders
{
    /// <summary>
    /// This class is responsible for loading the Game State (Elements).
    /// </summary>
    public class GameStateLoader : IGameStateLoader
    {
        //TODO: These should be a more accurate/specific type (Hashtable, maybe?)...using List for expediency
        private List<string> _allElementIds;
        private List<string> _allActionIds;

        public GameStateLoader()
        {
            this._allElementIds = new List<string>();
            this._allActionIds = new List<string>();
        }

        /// <summary>
        /// This method returns loaded GameState from the provided raw XML objects.
        /// Basic conversion and transformation logic should be done here.
        /// Any data issues should raise exceptions here.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public List<GameElement> LoadGameState(List<Element> elements)
        {
            try
            {
                var gameState = new List<GameElement>();
                foreach (var element in elements)
                {
                    gameState.Add(this.GetGameElement(element));
                }

                return gameState;
            }
            catch (Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("An exception occurred while loading the GameState.", ex));
            }
        }

        private GameElement GetGameElement(Element element)
        {
            //Make sure another element with the same Id doesn't exist
            if(this._allElementIds.Contains(element.Id))
            {
                throw new Exception(string.Format("An Element with the Id {0} already exists. Each Element Id must be unique. Check your configuration and retry.", element.Id));
            }
            else
            {
                this._allElementIds.Add(element.Id);
            }

            var gameElement = new GameElement()
            {
                Id = element.Id,
                Value = element.Value,
                Action = this.GetGameAction(element),
                Trigger = this.GetGameTrigger(element),
                Elements = this.GetChildElements(element)
            };

            return gameElement;
        }

        private GameAction GetGameAction(Element element)
        {
            GameAction action = null;
            if (element.Action != null)
            {
                //Make sure another action with the same Id doesn't exist
                if (this._allActionIds.Contains(element.Action.Id))
                {
                    throw new Exception(string.Format("An Action with the Id {0} already exists (Element Id {1}). Each Action Id must be unique. Check your configuration and retry.", element.Action.Id, element.Id));
                }
                else
                {
                    this._allActionIds.Add(element.Action.Id);
                }

                action = new GameAction()
                {
                    Id = element.Action.Id,
                    Setters = this.GetGameSetters(element.Id, element.Action.Setters)
                };
            }
            return action;
        }

        private GameTrigger GetGameTrigger(Element element)
        {
            GameTrigger gameTrigger = null;

            if (element.Trigger != null)
            {
                gameTrigger = new GameTrigger()
                {
                    TargetId = element.Trigger.Target,
                    Comparison = element.Trigger.Comparison,
                    Value = element.Trigger.Value,
                    Setters = this.GetGameSetters(element.Id, element.Trigger.Setters)
                };
            }

            return gameTrigger;
        }

        private List<GameSetter> GetGameSetters(string parentElementId, Setter[] setters)
        {
            var gameSetters = new List<GameSetter>();

            if(setters != null)
            {
                foreach(var setter in setters)
                {
                    gameSetters.Add(new GameSetter()
                    {
                        Operation = setter.Operation,
                        TargetId = setter.Target,
                        Value = setter.Value
                    });
                }
            }

            return gameSetters;
        }

        private List<GameElement> GetChildElements(Element element)
        {
            var childElements = new List<GameElement>();
            if (element.Elements != null)
            {
                //Make recursive call to populate nested game elements
                foreach (var innerElement in element.Elements)
                {
                    childElements.Add(this.GetGameElement(innerElement));
                }
            }

            return childElements;
        }
    }
}
