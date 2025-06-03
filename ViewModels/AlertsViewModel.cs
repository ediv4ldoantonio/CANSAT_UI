using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CANSAT_UI.ViewModels;

public partial class AlertsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Alert> alerts;

    private readonly IDataRepository dataRepository;

    public AlertsViewModel(IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
        alerts = [];
    }

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            IsBusy = true;

            var rawData = await dataRepository.ReadAlert();

            foreach (var item in rawData)
                Alerts.Add(item);

            OnPropertyChanged(nameof(Alerts));
        }
        finally
        {
            IsBusy = false;
        }
    }
}
