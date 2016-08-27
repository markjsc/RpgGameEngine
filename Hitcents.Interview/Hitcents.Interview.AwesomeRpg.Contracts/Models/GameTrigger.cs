using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameTrigger
    {
        public GameTrigger()
        {
            this.Setters = new List<GameSetter>();
        }

        public string TargetId { get; set; }

        public string Comparison { get; set; }

        public int Value { get; set; }

        public List<GameSetter> Setters { get; set; }
    }
}
