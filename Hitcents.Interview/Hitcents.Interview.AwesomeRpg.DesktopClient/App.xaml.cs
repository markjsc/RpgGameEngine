using System.Windows;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Engine;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IGameContext GameContext { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            GameContext = new GameContext();

            base.OnStartup(e);
        }
    }
}
