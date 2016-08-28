using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Engine
{
    /// <summary>
    /// This is where the Game State is stored, Actions are performed, and Triggers are fired.
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
                
                //TODO: Add a check here to validate that all Targets point to valid Element Ids so that we can trust Setters not to fail?
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
                var actionToRun = this.GetActionById(actionId);
                if(actionToRun != null)
                {
                    this.RunActionSetters(actionToRun.Setters, actionId);
                    this.RunTriggersFromActionSetters(actionToRun.Setters, actionId);             
                }
                else
                {
                    throw new Exception(string.Format("Unable to locate Action with Id {0} in the current Game Context. Check your configuration.", actionId));
                }
            }
            catch(Exception ex)
            {
                //TODO: Capture this with the logging mechanism
                throw new Exception(string.Format("Running the action for Action with Id {0} has failed.", actionId), ex);
            }
        }
                
        private void RunActionSetters(List<GameSetter> setters, string actionId)
        {
            foreach(var setter in setters)
            {
                var elementToSet = this.GetElementById(setter.TargetId);
                if(elementToSet != null)
                {
                    elementToSet.Value = this.CalculateNewElementValue(elementToSet.Value, setter.Operation, setter.Value, setter.TargetId);
                }
                else
                {
                    throw new Exception(string.Format("Unable to locate Element with Id {0}, defined as Target of Setter in Action with Id {1}.", setter.TargetId, actionId));
                }
            }
        }

        private void RunTriggersFromActionSetters(List<GameSetter> actionSetters, string actionId)
        {
            foreach(var actionSetter in actionSetters)
            {
                var trigger = this.GetTriggerByTarget(actionSetter.TargetId);
                if(trigger != null && this.ShouldTriggerRun(trigger))
                {
                    this.RunTriggerSetters(trigger.Setters, trigger.TargetId);
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

        private GameTrigger GetTriggerByTarget(string targetId)
        {
            return this._gameStateNavigator.GetTriggerByTarget(this._gameState, targetId);
        }

        /// <summary>
        /// Uses the Float type to determine whether the value is numeric.
        /// This supports both whole and partial numbers (as opposed to using
        /// integer).
        /// </summary>
        private bool IsNumeric(string value)
        {
            float unused;
            return !string.IsNullOrEmpty(value) &&
                   float.TryParse(value, out unused);
        }
    }
}
