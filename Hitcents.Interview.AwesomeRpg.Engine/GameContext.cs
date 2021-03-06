﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Engine
{
    /// <summary>
    /// This is where the Game State is stored, Actions are performed, and Triggers are fired.
    /// I know the logic is very iteration-driven. I believe this could be improved greatly
    /// (perhaps registering an event for each Element and then firing the event when an Action
    /// with a matching Target is performed). For this initial effort, I was focused on the logical 
    /// structure of the entities more than optimizing them for performance.
    /// </summary>
    public class GameContext : IGameContext
    {
        private List<GameElement> _gameState;
        private readonly IGameStateNavigator _gameStateNavigator;

        public GameContext(IGameStateNavigator gameStateNavigator)
        {
            this._gameState = new List<GameElement>();
            this._gameStateNavigator = gameStateNavigator;
        }

        public IReadOnlyCollection<GameElement> GameState
        {
            get
            {
                return new ReadOnlyCollection<GameElement>(this._gameState);
            }
        }

        public void BuildGameContext(List<GameElement> gameState)
        {
            try
            {
                this._gameState = gameState;
            }
            catch (Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception("Buliding the Game Context has failed.", ex);
            }
        }

        public void RunAction(string actionId)
        {
            try
            {
                var actionToRun = this.GetActionById(actionId);
                actionToRun.RunCount++;
                if (actionToRun != null)
                {
                    this.RunActionSetters(actionToRun.Setters, actionId);

                    try
                    {
                        this.RunTriggersFromSetters(actionToRun.Setters);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An exception occurred attempting to run the Triggers.", ex);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unable to locate Action with Id {0} in the current Game Context. Check your configuration.", actionId));
                }                
            }
            catch (Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("Running the action for Action with Id {0} has failed.", actionId), ex);
            }
        }

        private void RunActionSetters(List<GameSetter> setters, string actionId)
        {
            foreach (var setter in setters)
            {
                var elementToSet = this.GetElementById(setter.TargetId);
                if (elementToSet != null)
                {
                    elementToSet.Value = this.CalculateNewElementValue(elementToSet.Value, setter.Operation, setter.Value, setter.TargetId);
                }
                else
                {
                    throw new Exception(string.Format("Unable to locate Element with Id {0}, defined as Target of Setter in Action with Id {1}.", setter.TargetId, actionId));
                }
            }
        }

        private void RunTriggersFromSetters(List<GameSetter> setters)
        {
            foreach (var setter in setters)
            {
                this.RunTriggersOnElement(setter.TargetId);
            }
        }

        private void RunTriggersOnElement(string elementId)
        {
            var triggers = this.GetTriggersByTarget(elementId);
            foreach (var trigger in triggers)
            {
                if (this.ShouldTriggerRun(trigger))
                {
                    this.RunTriggerSetters(trigger.Setters, trigger.TargetId);
                }
                //Check to see if setter changed an Element that should fire another trigger
                foreach (var elementIdChangedBySetter in trigger.Setters.Select(x => x.TargetId))
                {
                    //Make recursive call to self.
                    //A stack overflow exception (aka infinite loop) is possible if a setter changes an element 
                    // that another trigger is watching and that trigger sets the same element the first trigger 
                    // was watching.
                    this.RunTriggersOnElement(elementIdChangedBySetter);
                }
            }

        }

        private void RunTriggerSetters(List<GameSetter> triggerSetters, string triggerTargetId)
        {
            foreach (var triggerSetter in triggerSetters)
            {
                var triggeredElement = this.GetElementById(triggerSetter.TargetId);
                if (triggeredElement == null)
                {
                    throw new Exception(string.Format("Unable to locate Element with Id {0}, defined as Target of Setter in Target with Id {1}.", triggerSetter.TargetId, triggerTargetId));
                }

                triggeredElement.Value = this.CalculateNewElementValue(triggeredElement.Value, triggerSetter.Operation, triggerSetter.Value, triggerTargetId);
            }
        }

        private bool ShouldTriggerRun(GameTrigger trigger)
        {
            bool shouldTriggerRun = false;
            var targetElement = this.GetElementById(trigger.TargetId);

            //
            //For Numeric comparisons (i.e. all except for Equal and Not Equal)
            //
            //Assume that if the Target Value is not numeric then its value is 0
            //Perhaps this should be checked by a validation method and an exception thrown when loading rather than using 0 here.
            float numericTargetValue = 0;
            float.TryParse(targetElement.Value, out numericTargetValue);
            //Assume that if the Trigger Value is not numeric then its value is 0
            //Perhaps this should be checked by a validation method and an exception thrown when loading rather than using 0 here.
            float numericTriggerValue = 0;
            float.TryParse(trigger.Value, out numericTriggerValue);

            switch (trigger.Comparison)
            {
                case TargetComparisons.Equal:
                    shouldTriggerRun = targetElement.Value == trigger.Value;
                    break;
                case TargetComparisons.NotEqual:
                    shouldTriggerRun = targetElement.Value != trigger.Value;
                    break;

                case TargetComparisons.GreaterThan:
                    shouldTriggerRun = numericTargetValue > numericTriggerValue;
                    break;
                case TargetComparisons.GreaterThanOrEqual:
                    shouldTriggerRun = numericTargetValue >= numericTriggerValue;
                    break;
                case TargetComparisons.LessThan:
                    shouldTriggerRun = numericTargetValue < numericTriggerValue;
                    break;
                case TargetComparisons.LessThanOrEqual:
                    shouldTriggerRun = numericTargetValue <= numericTriggerValue;
                    break;

                default:
                    throw new Exception(string.Format("The Comparison {0} is not recognized, specified in the Trigger with Target {1}.", trigger.Comparison, trigger.TargetId));
            }

            return shouldTriggerRun;
        }

        private string CalculateNewElementValue(string currentValue, string operation, string setterValue, string targetId)
        {
            string newElementValue = null;

            //
            //For Numeric operations (i.e. all except for Assign)
            //
            //Assume that if current Value is not numeric then its value is 0.
            //Perhaps this should be checked by a validation method and an exception thrown when loading rather than using 0 here.
            float currentNumericValue = 0;
            float.TryParse(currentValue, out currentNumericValue);
            //Assume that if setter Value is not numeric then its value is 0.
            //Perhaps this should be checked by a validation method and an exception thrown when loading rather than using 0 here.
            float setterNumericValue = 0;
            float.TryParse(setterValue, out setterNumericValue);

            switch (operation)
            {
                case SetterOperations.Assign:
                    newElementValue = setterValue;
                    break;

                case SetterOperations.Add:
                    newElementValue = (currentNumericValue + setterNumericValue).ToString();
                    break;

                case SetterOperations.Divide:
                    //In order to prevent Divide By Zero errors, substitute 1 if numeric value is zero.
                    setterNumericValue = setterNumericValue == 0 ? 1 : setterNumericValue;

                    newElementValue = (currentNumericValue / setterNumericValue).ToString();
                    break;

                case SetterOperations.Multiply:
                    newElementValue = (currentNumericValue * setterNumericValue).ToString();
                    break;

                case SetterOperations.Subtract:
                    newElementValue = (currentNumericValue - setterNumericValue).ToString();
                    break;

                default:
                    throw new Exception(string.Format("The Operation {0} is not recognized, specified in Setter with Target {1}.", operation, targetId));
            }

            return newElementValue;
        }

        private GameElement GetElementById(string id)
        {
            return this._gameStateNavigator.GetElementById(this._gameState, id);
        }

        private GameAction GetActionById(string id)
        {
            return this._gameStateNavigator.GetActionById(this._gameState, id);
        }

        private List<GameTrigger> GetTriggersByTarget(string targetId)
        {
            return this._gameStateNavigator.GetTriggersByTarget(this._gameState, targetId);
        }
    }
}
