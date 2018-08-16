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

            if (elements.Any(x => AreStringsEqual(id, x.Id)))
            {
                gameElement = elements.First(x => AreStringsEqual(id, x.Id));
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

            if (elements.Any(x => x.Actions.Any(y => AreStringsEqual(actionId, y.Id))))
            {
                var element = elements.First(x => x.Actions.Any(y => AreStringsEqual(actionId, y.Id)));
                action = element.Actions.First(x => AreStringsEqual(actionId, x.Id));
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

            foreach(var element in elements)
            {
                foreach (var trigger in element.Triggers.Where(x => AreStringsEqual(targetId, x.TargetId)))
                {
                    triggers.Add(trigger);
                }

                if (element.Elements != null)
                {
                    triggers.AddRange(this.GetTriggersByTarget(element.Elements, targetId));
                }
            }        

            return triggers;
        }

        private bool AreStringsEqual(string a, string b)
        {
            //Assume we're OK with case-insensitivity
            return string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
