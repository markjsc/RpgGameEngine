using System;
using System.Collections.Generic;
using System.Linq;
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

        public GameContext()
        {
            this._gameState = new List<GameElement>();            
        }

        public List<GameElement> GameState
        {
            get
            {
                return this._gameState;
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
            //Assume that if the target Element or its value is NULL then 0 will be used for comparison
            var assumedElementValue = targetElement != null && targetElement.Value != null ? targetElement.Value : 0;

            switch (trigger.Comparison)
            {
                case TargetComparisons.Equal:
                    shouldTriggerRun = assumedElementValue == trigger.Value;
                    break;
                case TargetComparisons.GreaterThan:
                    shouldTriggerRun = assumedElementValue > trigger.Value;
                    break;
                case TargetComparisons.GreaterThanOrEqual:
                    shouldTriggerRun = assumedElementValue >= trigger.Value;
                    break;
                case TargetComparisons.LessThan:
                    shouldTriggerRun = assumedElementValue < trigger.Value;
                    break;
                case TargetComparisons.LessThanOrEqual:
                    shouldTriggerRun = assumedElementValue <= trigger.Value;
                    break;
                case TargetComparisons.NotEqual:
                    shouldTriggerRun = assumedElementValue != trigger.Value;
                    break;
                default:
                    throw new Exception(string.Format("The Comparison {0} is not recognized, specified in the Trigger with Target {1}.", trigger.Comparison, trigger.TargetId));
            }

            return shouldTriggerRun;
        }

        private int? CalculateNewElementValue(int? currentValue, string operation, int setterValue, string targetId)
        {
            int? newElementValue = null;
            //Assume that if current Value is null, treat it like 0
            var assumedCurrentValue = currentValue != null ? currentValue.Value : 0;

            switch (operation)
            {
                case SetterOperations.Add:
                    newElementValue = assumedCurrentValue + setterValue;
                    break;

                case SetterOperations.Assign:
                    newElementValue = setterValue;
                    break;

                case SetterOperations.Divide:
                    newElementValue = assumedCurrentValue / setterValue;
                    break;

                case SetterOperations.Multiply:
                    newElementValue = assumedCurrentValue * setterValue;
                    break;

                case SetterOperations.Subtract:
                    newElementValue = assumedCurrentValue - setterValue;
                    break;

                default:
                    throw new Exception(string.Format("The Operation {0} is not recognized, specified in Setter with Target {1}.", operation, targetId));
            }

            return newElementValue;
        }
       
        private GameElement GetElementById(string id)
        {
            return this.GetElementById(this._gameState, id);
        }

        private GameElement GetElementById(List<GameElement> elements, string id)
        {
            GameElement gameElement = null;

            if(elements.Any(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase)))
            {
                gameElement = elements.First(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                foreach(var element in elements)
                {
                    if(element.Elements != null)
                    {
                        gameElement = this.GetElementById(element.Elements, id);
                    }
                }
            }

            return gameElement;
        }

        private GameAction GetActionById(string id)
        {
            return this.GetActionById(this._gameState, id);
        }

        private GameAction GetActionById(List<GameElement> elements, string id)
        {
            GameAction action = null;
            
            if(elements.Any(x => x.Action != null && string.Equals(id, x.Action.Id, StringComparison.CurrentCultureIgnoreCase)))
            {
                action = elements.First(x => x.Action != null && string.Equals(id, x.Action.Id, StringComparison.CurrentCultureIgnoreCase)).Action;
            }
            else
            {
                foreach(var element in elements)
                {
                    if(element.Elements != null)
                    {
                        action = this.GetActionById(element.Elements, id);
                    }
                }
            }

            return action;
        }

        private GameTrigger GetTriggerByTarget(string targetId)
        {
            return this.GetTriggerByTarget(this._gameState, targetId);
        }

        private GameTrigger GetTriggerByTarget(List<GameElement> elements, string targetId)
        {
            GameTrigger trigger = null;

            if(elements.Any(x => x.Trigger != null && string.Equals(targetId, x.Trigger.TargetId, StringComparison.CurrentCultureIgnoreCase)))
            {
                trigger = elements.First(x => x.Trigger != null && string.Equals(targetId, x.Trigger.TargetId, StringComparison.CurrentCultureIgnoreCase)).Trigger;
            }
            else
            {
                foreach(var element in elements)
                {
                    if(element.Elements != null)
                    {
                        trigger = this.GetTriggerByTarget(element.Elements, targetId);
                    }
                }
            }

            return trigger;
        }
    }
}
