using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CANSAT_UI.ViewModels;

public partial class DatabaseViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Data> data;

    private readonly IDataRepository dataRepository;

    public DatabaseViewModel(IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
        data = [];
    }

    [RelayCommand]
    private async Task LoadData()
    {
        try
        {
            IsBusy = true;

            var rawData = await dataRepository.Read();

            foreach (var item in rawData)
                Data.Add(item);

            OnPropertyChanged(nameof(Data));
        }
        finally
        {
            IsBusy = false;
        }
    }
}
