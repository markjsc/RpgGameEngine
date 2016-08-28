using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameElement
    {
        public GameElement()
        {
            this.Elements = new List<GameElement>();
        }

        public string Id { get; set; }

        /// <summary>
        /// This can be either numeric or string. The comparisons and operations check
        /// the type before acting and will only perform the comparison or operation
        /// if the type is appropriate.
        /// </summary>
        public string Value { get; set; }

        public List<GameElement> Elements { get; set; }

        public GameAction Action { get; set; }
        
        public GameTrigger Trigger { get; set; }

        public override string ToString()
        {
            //TODO: Implement a formatted string output to show the tree of Elements, Actions, Triggers and Setters
            return base.ToString();
        }
    }
}
