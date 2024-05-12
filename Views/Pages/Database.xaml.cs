using CANSAT_UI.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;


namespace CANSAT_UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Database.xaml
    /// </summary>
    public partial class Database : Page, INavigableView<DatabaseViewModel>
    {
        public Database(DatabaseViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        public DatabaseViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.LoadDataCommand.ExecuteAsync(this);
        }
    }
}
