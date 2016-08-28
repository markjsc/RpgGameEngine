using System;
using System.Collections.ObjectModel;
using System.Windows;
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
            //DataContext = this;
        }
        
        public ObservableCollection<GameElement> GameState
        {
            get { return (ObservableCollection<GameElement>)GetValue(GameStateProperty); }
            set { SetValue(GameStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameStateProperty =
            DependencyProperty.Register("GameState", typeof(ObservableCollection<GameElement>), typeof(GameStateViewer), new PropertyMetadata(new ObservableCollection<GameElement>()));
        
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
