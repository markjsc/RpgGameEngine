using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Hitcents.Interview.AwesomeRpg.Loaders;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.UserControls
{
    /// <summary>
    /// Interaction logic for LoadXmlControl.xaml
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

        private void LoadFromFileButton_Click(object sender, System.Windows.RoutedEventArgs e)
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

        private void LoadGameStateButton_Click(object sender, System.Windows.RoutedEventArgs e)
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
