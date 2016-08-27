namespace Hitcents.Interview.AwesomeRpg.Contracts.Models
{
    public class GameSetter
    {
        public string TargetId { get; set; }

        public string Operation { get; set; }

        /// <summary>
        /// This is assume to be an integer for the purpose of this interview project.
        /// In real life this could be any type and the triggering logic would need to
        /// be adjusted to account for operations against varying types at runtime.
        /// </summary>
        public int Value { get; set; }
    }
}
