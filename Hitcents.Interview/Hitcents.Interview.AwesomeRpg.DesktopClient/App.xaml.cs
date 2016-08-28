using System.Windows;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Engine;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient
{
    public partial class App : Application
    {
        /// <summary>
        /// Static holder for GameContext
        /// In a larger system this would be managed by a Dependency Injection container (i.e. Unity, CastleWindsor, etc).
        /// I decided to keep it simple in the interest of time.
        /// </summary>
        public static IGameContext GameContext { get; set; }

        /// <summary>
        /// Startup logic
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            GameContext = new GameContext();

            base.OnStartup(e);
        }
    }
}
