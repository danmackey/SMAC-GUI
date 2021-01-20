using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SMAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly string help = "https://github.com/danmackey/SMAC";
        private readonly Controller controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// If there is an exception raised while checking the config file,
        /// an error message is displayed and the application closes.
        /// Otherwise, the application initializes and runs.
        /// </summary>
        public MainWindow()
        {
            DataContext = this;

            try
            {
                controller = new Controller();
                controller.LoadSettingsFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex}", "Sim-Monsters Anti Cheat");
                Application.Current.Shutdown();
            }

            InitializeComponent();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the current server message displayed on GUI.
        /// </summary>
        public string CurrentServerMessage => $"Currently Connected to: {(IsSmServerSelected ? "Sim-Monsters Anti Cheat" : "Rigs of Rods")}";

        private bool IsSmServerSelected
        {
            get => controller.IsSmServerSelected;

            set => controller.IsSmServerSelected = value;
        }

        /// <summary>
        /// Performs <see cref="PropertyChanged"/> event on the provided proptery.
        /// </summary>
        /// <param name="propertyName">Property name that the event will be triggered on.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetCurrentServer()
        {
            try
            {
                if (controller.ApplyConfigChanges())
                {
                    controller.SaveSettingsFile();
                    OnPropertyChanged(nameof(CurrentServerMessage));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex}", "Sim-Monsters Anti Cheat");
            }
        }

        private void OnHelpClick(object sender, RoutedEventArgs e)
        {
            Process openHelpPage = new Process();
            try
            {
                openHelpPage.StartInfo.UseShellExecute = true;
                openHelpPage.StartInfo.FileName = help;
                openHelpPage.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        private void OnSimMonstersClick(object sender, RoutedEventArgs e)
        {
            IsSmServerSelected = true;
            SetCurrentServer();
        }

        private void OnRigsOfRodsClick(object sender, RoutedEventArgs e)
        {
            IsSmServerSelected = false;
            SetCurrentServer();
        }
    }
}