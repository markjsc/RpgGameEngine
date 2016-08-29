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

            if (elements.Any(x => x.Actions.Any(y => string.Equals(actionId, y.Id, StringComparison.CurrentCultureIgnoreCase))))
            {
                var element = elements.First(x => x.Actions.Any(y => string.Equals(actionId, y.Id, StringComparison.CurrentCultureIgnoreCase)));
                action = element.Actions.First(x => string.Equals(actionId, x.Id, StringComparison.CurrentCultureIgnoreCase));
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
       
        public List<GameTrigger> GetTriggersByTarget(List<GameElement> elements, string targetId)
        {
            List<GameTrigger> triggers = new List<GameTrigger>();

            if (elements.Any(x => x.Triggers.Any(y => string.Equals(targetId, y.TargetId, StringComparison.CurrentCultureIgnoreCase))))
            {
                var element = elements.First(x => x.Triggers.Any(y => string.Equals(targetId, y.TargetId, StringComparison.CurrentCultureIgnoreCase)));
                foreach (var trigger in element.Triggers.Where(x => string.Equals(targetId, x.TargetId, StringComparison.CurrentCultureIgnoreCase)))
                {
                    triggers.Add(trigger);
                }
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element.Elements != null)
                    {
                        foreach(var trigger in this.GetTriggersByTarget(element.Elements, targetId))
                        {
                            triggers.Add(trigger);
                        }

                    }
                }
            }

            return triggers;
        }
    }
}
