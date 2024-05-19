using CANSAT_UI.ViewModels;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace CANSAT_UI.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : FluentWindow, INavigationWindow
    {
        public ContainerViewModel ViewModel { get; }

        public Container(ContainerViewModel viewModel, INavigationService navigationService, IPageService pageService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            // We define a page provider f or navigation
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }


        #region INavigationWindow methods

        public void CloseWindow() => Close();

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow() => Show();

        #endregion INavigationWindow methods
    }
}
