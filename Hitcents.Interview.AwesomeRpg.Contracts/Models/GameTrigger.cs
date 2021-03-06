﻿using System.Collections.Generic;

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

        /// <summary>
        /// This can be either numeric or string. The comparisons and operations check
        /// the type before acting and will only perform the comparison or operation
        /// if the type is appropriate.
        /// </summary>
        public string Value { get; set; }
        
        public List<GameSetter> Setters { get; set; }
    }
}
