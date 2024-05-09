using CANSAT_UI.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;


namespace CANSAT_UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : Page, INavigableView<ConnectionViewModel>
    {
        public Connection(ConnectionViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
        }

        public ConnectionViewModel ViewModel { get; }
    }
}
