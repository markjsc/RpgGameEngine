using System;
using System.Collections.Generic;
using System.Linq;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.Engine
{
    public class GameStateNavigator : IGameStateNavigator
    {
        public GameElement GetElementById(List<GameElement> elements, string id)
        {
            GameElement gameElement = null;

            if (elements.Any(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase)))
            {
                gameElement = elements.First(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element.Elements != null)
                    {
                        gameElement = this.GetElementById(element.Elements, id);
                    }
                }
            }

            return gameElement;
        }

        public GameAction GetActionById(List<GameElement> elements, string actionId)
        {
            GameAction action = null;

            if (elements.Any(x => x.Action != null && string.Equals(actionId, x.Action.Id, StringComparison.CurrentCultureIgnoreCase)))
            {
                action = elements.First(x => x.Action != null && string.Equals(actionId, x.Action.Id, StringComparison.CurrentCultureIgnoreCase)).Action;
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element.Elements != null)
                    {
                        action = this.GetActionById(element.Elements, actionId);
                    }
                }
            }

            return action;
        }
       
        public GameTrigger GetTriggerByTarget(List<GameElement> elements, string targetId)
        {
            GameTrigger trigger = null;

            if (elements.Any(x => x.Trigger != null && string.Equals(targetId, x.Trigger.TargetId, StringComparison.CurrentCultureIgnoreCase)))
            {
                trigger = elements.First(x => x.Trigger != null && string.Equals(targetId, x.Trigger.TargetId, StringComparison.CurrentCultureIgnoreCase)).Trigger;
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element.Elements != null)
                    {
                        trigger = this.GetTriggerByTarget(element.Elements, targetId);
                    }
                }
            }

            return trigger;
        }
    }
}
