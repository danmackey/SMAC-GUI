using System.ComponentModel;
using System.Windows;

namespace SMAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Controller controller;
        private string _currentServerMessage;
        public string CurrentServerMessage
        {
            get
            {
                return _currentServerMessage;
            }
            set
            {
                if (value != _currentServerMessage)
                {
                    _currentServerMessage = value;
                    OnPropertyChanged("CurrentServerMessage");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
        public MainWindow()
        {
            DataContext = this;
            controller = new Controller();
            GetCurrentServer();
            InitializeComponent();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetCurrentServer()
        {
            bool isSMApiServer = controller.settings.smApiServerActive;
            CurrentServerMessage = $"Currently Connected to: {(isSMApiServer ? "Sim-Monsters Anti Cheat" : "Rigs of Rods")}";
        }

        private void SetCurrentServer(bool isSMApiServer)
        {
            if (controller.SwitchServers(isSMApiServer))
            {
                CurrentServerMessage = $"Currently Connected to: {(isSMApiServer ? "Sim-Monsters Anti Cheat" : "Rigs of Rods")}";
            }
        }

        private void btnSimMonsters_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentServer(true);
        }

        private void btnRigsOfRods_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentServer(false);
        }
    }
}
