using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.UserControls
{
    public partial class GameStateViewer : UserControl
    {
        public event EventHandler NavigateBackEvent;

        public GameStateViewer()
        {
            InitializeComponent();
        }
        
        public ObservableCollection<GameElement> GameState
        {
            get { return (ObservableCollection<GameElement>)GetValue(GameStateProperty); }
            set { SetValue(GameStateProperty, value); }
        }

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

        private void DoActionButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Make this more elegant - it works, but it's not MVVM'y!
            try
            {
                var actionId = (sender as Button).Tag.ToString();
                App.GameContext.RunAction(actionId);
                //Freshen up the screen
                this.GameState = new ObservableCollection<GameElement>(App.GameContext.GameState);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Unable to run the Action. Exception:{0}", ex));
            }
        }
    }
}
