using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameElement
    {
        public GameElement()
        {
            this.Elements = new List<GameElement>();
        }

        /// <summary>
        /// Unique Identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// This is assume to be an integer for the purpose of this interview project.
        /// In real life this could be any type and the triggering logic would need to
        /// be adjusted to account for operations against varying types at runtime.
        /// </summary>
        public int? Value { get; set; }

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
