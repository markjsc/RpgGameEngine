using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Hitcents.Interview.AwesomeRpg.Loaders;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.UserControls
{
    /// <summary>
    /// In a bigger system there would be almost NO code here - it would live in ViewModels.
    /// For this sample, it's much quicker to implement a few small bits of interaction here.
    /// Also, I did not create any Unit Tests for the WPF project since there is no business logic here.
    /// In a larger system, any non-trivial logic (business logic, navigation, etc) would be fully tested!
    /// </summary>
    public partial class LoadXmlControl : UserControl
    {
        public event EventHandler GameStateLoaded;

        public LoadXmlControl()
        {
            InitializeComponent();
        }

        public void ClearGameXml()
        {
            this.XmlTextBox.Text = null;

        }

        private void LoadFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            var fileOpen = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".xml",
                Filter = "XML File (.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Select the XML file containing the Game State",
                CheckFileExists = true
            };
            var fileOpenResult = fileOpen.ShowDialog();

            if (fileOpenResult != null && fileOpenResult.Value)
            {
                try
                {
                    XmlTextBox.Text = File.ReadAllText(fileOpen.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("An exception occurred opening the XML file ({0}). Exception: {1}", fileOpen.FileName, ex), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadGameStateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loader = new XmlLoader();
                var gameStateLoader = new GameStateLoader();

                var rawGameState = loader.LoadXml(XmlTextBox.Text);
                var gameState = gameStateLoader.LoadGameState(rawGameState);

                App.GameContext.BuildGameContext(gameState);
                this.OnGameStateLoaded();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An exception occurred loading the Game State. Exception: {0}", ex), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnGameStateLoaded()
        {
            if(this.GameStateLoaded != null)
            {
                this.GameStateLoaded.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
