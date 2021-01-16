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
        public bool IsSimMonstersApiServer => controller.settings.smApiServerActive;
        public string CurrentServerMessage => $"Currently Connected to: {(IsSimMonstersApiServer ? "Sim-Monsters Anti Cheat" : "Rigs of Rods")}";

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            DataContext = this;
            controller = new Controller();
            InitializeComponent();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetCurrentServer(bool isSMApiServer)
        {
            _ = controller.SwitchServers(isSMApiServer);
            OnPropertyChanged(nameof(CurrentServerMessage));
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
