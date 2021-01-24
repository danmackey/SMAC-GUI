using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
                if (controller.LoadSettingsFile())
                {
                    ProgressBarVisibility = Visibility.Hidden;
                    OnPropertyChanged(nameof(ProgressBarVisibility));
                    InitializeComponent();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ExceptionMessageBox(ex.Message);
                Application.Current.Shutdown();
                Close();
                Environment.Exit(0);
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the current server message displayed on GUI.
        /// </summary>
        public string CurrentServerMessage => $"Connected to: {(IsSmServerSelected ? "Sim-Monsters Anti Cheat" : "Rigs of Rods")}";

        /// <summary>
        /// Gets the current server url to display on GUI.
        /// </summary>
        public string CurrentServerUrl => $"API Server URL: {controller.CurrentServerUrl}";

        /// <summary>
        /// Gets or sets the visibility of the progress bar.
        /// </summary>
        public Visibility ProgressBarVisibility { get; set; }

        /// <summary>
        /// Gets or sets the value of the progress bar.
        /// </summary>
        public int ProgressBarValue { get; set; }

        /// <inheritdoc cref="SettingsJson.IsSmServerSelected"/>
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

        /// <summary>
        /// Shows progress bar to show that the API server was changed.
        /// Runs asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ProgressBarComplete()
        {
            ProgressBarVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(ProgressBarVisibility));
            for (int i = 0; i <= 20; i++)
            {
                ProgressBarValue = i * 5;
                OnPropertyChanged(nameof(ProgressBarValue));
                await Task.Delay(1);
            }

            ProgressBarVisibility = Visibility.Hidden;
            OnPropertyChanged(nameof(ProgressBarVisibility));
        }

        /// <summary>
        /// Attempts to set the current API server.
        /// If the changes are applied to RoR.cfg, then settings.json is saved,
        /// and the GUI is updated accordingly.
        /// </summary>
        private void SetCurrentServer()
        {
            try
            {
                if (controller.ApplyConfigChanges())
                {
                    controller.SaveSettingsFile();
                    _ = ProgressBarComplete();
                    OnPropertyChanged(nameof(CurrentServerMessage));
                    OnPropertyChanged(nameof(CurrentServerUrl));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ExceptionMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Attempts to open help page, which is the GitHub README.
        /// </summary>
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
                Debug.WriteLine(ex);
                ExceptionMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Sets the API Server to the Sim-Monsters API server.
        /// </summary>
        private void OnSimMonstersClick(object sender, RoutedEventArgs e)
        {
            IsSmServerSelected = true;
            SetCurrentServer();
        }

        /// <summary>
        /// Sets the API Server to the Rigs of Rods API server.
        /// </summary>
        private void OnRigsOfRodsClick(object sender, RoutedEventArgs e)
        {
            IsSmServerSelected = false;
            SetCurrentServer();
        }

        /// <summary>
        /// Display <see cref="MessageBox"/> containing an exception message.
        /// </summary>
        /// <param name="message">Message to be displayed in <see cref="MessageBox"/>.</param>
        private void ExceptionMessageBox(string message)
        {
            MessageBox.Show(message, "Sim-Monsters Anti Cheat");
        }
    }
}