using CommunityToolkit.Mvvm.ComponentModel;

namespace CANSAT_UI.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;
    }
}
