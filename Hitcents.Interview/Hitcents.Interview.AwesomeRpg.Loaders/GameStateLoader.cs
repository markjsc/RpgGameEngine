﻿using System;
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
                    gameState.Add(this.GetGameElement(gameState, element));
                }

                return gameState;
            }
            catch (Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("An exception occurred while loading the GameState.", ex));
            }
        }

        private GameElement GetGameElement(List<GameElement> readOnlyGameState, Element element)
        {
            var gameElement = new GameElement()
            {
                Id = element.Id,
                Value = this.GetElementValue(element.Id, element.Value),
                Action = this.GetGameAction(element),
                Trigger = this.GetGameTrigger(element),
                Elements = this.GetChildElements(readOnlyGameState, element)
            };

            return gameElement;
        }

        private int? GetElementValue(string parentId, string elementValue)
        {
            //TODO: Consider changing this to support additional types besides integer (per a "maybe" in the specs)
            int? nullableElementValue = null;
            if (!string.IsNullOrEmpty(elementValue))
            {
                int parsedElementValue;
                if (int.TryParse(elementValue, out parsedElementValue))
                {
                    nullableElementValue = parsedElementValue;
                }
                else
                {
                    throw new Exception(string.Format("Unable to parse Value ({0}) as integer for Element with Id {1}.", elementValue, parentId));
                }
            }
            return nullableElementValue;
        }

        private GameAction GetGameAction(Element element)
        {
            GameAction action = null;
            if (element.Action != null)
            {
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
                //TODO: Consider changing this to support additional types besides integer (per a "maybe" in the specs)
                int parsedTriggerValue;
                if (!int.TryParse(element.Trigger.Value, out parsedTriggerValue))
                {
                    throw new Exception(string.Format("Unable to parse Value ({0}) as integer for Trigger with Target {1}, belonging to Element with Id {2}.", element.Trigger.Value, element.Trigger.Target, element.Id));
                }

                gameTrigger = new GameTrigger()
                {
                    TargetId = element.Trigger.Target,
                    Comparison = element.Trigger.Comparison,
                    Value = parsedTriggerValue,
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
                    //TODO: Consider changing this to support additional types besides integer (per a "maybe" in the specs)
                    int parsedSetterValue;
                    if(!int.TryParse(setter.Value, out parsedSetterValue))
                    {
                        throw new Exception(string.Format("Unable to parse Value ({0}) as integer for Setter with Target {1}, belonging to Element with Id {2}.", setter.Value, setter.Target, parentElementId));
                    }

                    gameSetters.Add(new GameSetter()
                    {
                        Operation = setter.Operation,
                        TargetId = setter.Target,
                        Value = parsedSetterValue
                    });
                }
            }

            return gameSetters;
        }

        private List<GameElement> GetChildElements(List<GameElement> readOnlyGameState, Element element)
        {
            var childElements = new List<GameElement>();
            if (element.Elements != null)
            {
                //Make recursive call to populate nested game elements
                foreach (var innerElement in element.Elements)
                {
                    childElements.Add(this.GetGameElement(readOnlyGameState, innerElement));
                }
            }

            return childElements;
        }
    }
}
