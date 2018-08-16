using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.UserControls
{
    /// <summary>
    /// In a bigger system there would be almost NO code here - it would live in ViewModels.
    /// For this sample, it's much quicker to implement a few small bits of interaction here.
    /// Also, I did not create any Unit Tests for the WPF project since there is no business logic here.
    /// In a larger system, any non-trivial logic (business logic, navigation, etc) would be fully tested!
    /// </summary>
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
       
        private void BackButton_Click(object sender, RoutedEventArgs e)
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
                //Freshen up the screen (in an MVVM approach the screen's data source would be updated directly
                // instead of re-binding to a new instance after each Action is fired)
                this.GameState = new ObservableCollection<GameElement>(App.GameContext.GameState);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Unable to run the Action. Exception:{0}", ex));
            }
        }
    }
}
