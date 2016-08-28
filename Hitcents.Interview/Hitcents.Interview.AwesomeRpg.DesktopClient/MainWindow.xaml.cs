using System;
using System.Windows;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            GameStateViewer.Visibility = Visibility.Collapsed;
            //LoadXmlControl.ClearGameXml(); //Originally cleared the XML, but after using the client a bit, I decided NOT to clear it.
            LoadXmlControl.Visibility = Visibility.Visible;            
        }

        private void LoadXmlControl_GameStateLoaded(object sender, EventArgs e)
        {
            LoadXmlControl.Visibility = Visibility.Collapsed;
            GameStateViewer.GameState = new System.Collections.ObjectModel.ObservableCollection<Contracts.Models.GameElement>(App.GameContext.GameState);
            GameStateViewer.Visibility = Visibility.Visible;
        }
    }
}
