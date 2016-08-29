using System;
using System.Collections.Generic;
using System.Linq;
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

                this.ValidateTriggerTargetsAreValidElementIds(gameState);
                this.ValidateSetterTargetsAreValidElementIds(gameState);

                return gameState;
            }
            catch (Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("An exception occurred while loading the GameState.", ex));
            }
        }

        private void ValidateSetterTargetsAreValidElementIds(List<GameElement> gameState)
        {
            //TODO: Add some validation to make sure that all Setter Targets are valid Element Ids
        }

        private void ValidateTriggerTargetsAreValidElementIds(List<GameElement> gameState)
        {
            //TODO: Add some validation to make sure all Trigger Targets are valid Element Ids
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
                Actions = this.GetGameActions(element),
                Triggers = this.GetGameTriggers(element),
                Elements = this.GetChildElements(element)
            };

            return gameElement;
        }

        private List<GameAction> GetGameActions(Element element)
        {
            List<GameAction> actions = new List<GameAction>();
            if (element.Actions != null)
            {
                //Make sure another action with the same Id doesn't exist
                if (element.Actions.Any(x => this._allActionIds.Contains(x.Id)))
                {
                    throw new Exception(string.Format("A duplicate Action is being created from the ELement {0}. Each Action Id must be unique. Check your configuration and retry.", element.Id));
                }
                else
                {
                    this._allActionIds.AddRange(element.Actions.Select(x => x.Id));                    
                }

                foreach(var action in element.Actions)
                {
                    actions.Add(new GameAction()
                    {
                        Id = action.Id,
                        Setters = this.GetGameSetters(element.Id, action.Setters)
                    });
                }
            }
            return actions;
        }

        private List<GameTrigger> GetGameTriggers(Element element)
        {
            List<GameTrigger> gameTriggers = new List<GameTrigger>();

            if (element.Triggers != null)
            {
                foreach(var trigger in element.Triggers)
                {
                    gameTriggers.Add(new GameTrigger()
                    {
                        TargetId = trigger.Target,
                        Comparison = trigger.Comparison,
                        Value = trigger.Value,
                        Setters = this.GetGameSetters(element.Id, trigger.Setters)
                    });
                }
            }

            return gameTriggers;
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
