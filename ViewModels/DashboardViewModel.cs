using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using Timer = System.Timers.Timer;


namespace CANSAT_UI.ViewModels;


public partial class DashboardViewModel : ViewModelBase
{
    #region ObservableProperties
    [ObservableProperty]
    private float[] trashLevels;
    #endregion

    private readonly ISerialCommunicationService serialCommunicationService;
    private readonly IDataRepository dataRepository;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService, IDataRepository dataRepository)
    {
        this.trashLevels = new float[2];
        this.serialCommunicationService = serialCommunicationService;
        this.dataRepository = dataRepository;

        InitializeDataReceived();
    }

    private void InitializeDataReceived()
    {
        serialCommunicationService.DataReceived += OnDataReceived;
    }

    private async void OnDataReceived(object? sender, string data)
    {
        try
        {
            ResponseData? responseData = JsonSerializer.Deserialize<ResponseData>(data);

            if (responseData != null)
            {
                for (int i = 0; i < responseData.trashLevels.Length; i++)
                {
                    TrashLevels[i] = responseData.trashLevels[i];
                }

                OnPropertyChanged(nameof(TrashLevels));

                await SaveToDatabase();
            }
        }
        catch
        {

        }
    }

    private async Task SaveToDatabase()
    {
        for (int i = 0; i < TrashLevels.Length; i++)
        {
            await dataRepository.Create(new Data { Level = TrashLevels[i], Number = i + 1, Timestamp = DateTime.Now });
        }
    }
}