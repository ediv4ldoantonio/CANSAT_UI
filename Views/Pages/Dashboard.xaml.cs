using CANSAT_UI.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace CANSAT_UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page, INavigableView<DashboardViewModel>
    {
        public Dashboard(DashboardViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
        }

        public DashboardViewModel ViewModel { get; }
    }
}
