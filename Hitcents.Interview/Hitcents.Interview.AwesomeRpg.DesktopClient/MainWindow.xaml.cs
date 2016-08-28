using System;
using System.Windows;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient
{
    /// <summary>
    /// In a bigger system there would be almost NO code here - it would live in ViewModels.
    /// For this sample, it's much quicker to implement a few small bits of interaction here.
    /// Also, I did not create any Unit Tests for the WPF project since there is no business logic here.
    /// In a larger system, any non-trivial logic (business logic, navigation, etc) would be fully tested!
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadXmlControl.GameStateLoaded += LoadXmlControl_GameStateLoaded;
            GameStateViewer.NavigateBackEvent += GameStateViewer_NavigateBackEvent;
        }

        private void GameStateViewer_NavigateBackEvent(object sender, EventArgs e)
        {
            //This was originally called to clear the XML, but after using the client a bit, I decided it flowed better to keep the original XML in place.
            //LoadXmlControl.ClearGameXml(); 

            GameStateViewer.Visibility = Visibility.Collapsed;
            LoadXmlControl.Visibility = Visibility.Visible;            
        }

        private void LoadXmlControl_GameStateLoaded(object sender, EventArgs e)
        {
            GameStateViewer.GameState = new System.Collections.ObjectModel.ObservableCollection<Contracts.Models.GameElement>(App.GameContext.GameState);

            LoadXmlControl.Visibility = Visibility.Collapsed;            
            GameStateViewer.Visibility = Visibility.Visible;
        }
    }
}
