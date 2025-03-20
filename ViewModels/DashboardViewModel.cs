using CANSAT_UI.Models;
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
    private string airQuality = string.Empty;

    [ObservableProperty]
    private string humidity = string.Empty;

    [ObservableProperty]
    private string temperature = string.Empty;


    #endregion
    readonly HttpClient client;

    // O token do Blynk
    const string token = "Gxs-jRBc2MEKtDIRL1Up43pSYtfRu-od";

    private readonly ISerialCommunicationService serialCommunicationService;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService)
    {
        this.client = new HttpClient();

        this.serialCommunicationService = serialCommunicationService;

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
                Humidity = $"{responseData.humidity}%";
                AirQuality = $"{responseData.airQuality * 100 / 1023}%"; ;
                Temperature = $"{responseData.temperature} ºC";

                OnPropertyChanged(nameof(Humidity));
                OnPropertyChanged(nameof(AirQuality));
                OnPropertyChanged(nameof(Temperature));

                await sendToBlynk();
            }
        }
        catch
        {

        }
    }

    private async Task sendToBlynk()
    {
        string pinoVirtual1 = "v0";
        string pinoVirtual2 = "v1";
        string pinoVirtual3 = "v2";


        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual1}={Humidity}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual2}={Temperature}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual3}={AirQuality}");
    }
}