using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient
{
    /// <summary>
    /// Interaction logic for GameStateViewer.xaml
    /// </summary>
    public partial class GameStateViewer : UserControl
    {
        public event EventHandler NavigateBackEvent;

        public GameStateViewer()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public ObservableCollection<GameElement> GameState
        {
            get
            {
                return new ObservableCollection<GameElement>(App.GameContext.GameState);
            }
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OnNavigateBack();
        }

        private void OnNavigateBack()
        {
            if(this.NavigateBackEvent != null)
            {
                this.NavigateBackEvent.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
