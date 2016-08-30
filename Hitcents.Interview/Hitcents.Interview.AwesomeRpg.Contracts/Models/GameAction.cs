using System.Collections.Generic;

namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameAction
    {
        public GameAction()
        {
            this.Setters = new List<GameSetter>();
            this.RunCount = 0;
        }

        public string Id { get; set; }

        public List<GameSetter> Setters { get; set; }

        public int RunCount { get; set; }
    }
}
