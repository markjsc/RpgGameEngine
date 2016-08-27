namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameTrigger
    {
        public GameElement Target { get; set; }

        public string Comparison { get; set; }

        public int Value { get; set; }

        public GameSetter Setter { get; set; }
    }
}
