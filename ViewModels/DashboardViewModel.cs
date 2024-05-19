using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace CANSAT_UI.ViewModels;


public partial class DashboardViewModel : ViewModelBase
{
    #region ObservableProperties
    [ObservableProperty]
    private string location = string.Empty;

    [ObservableProperty]
    private string date = string.Empty;

    [ObservableProperty]
    private string time = string.Empty;

    [ObservableProperty]
    private string temperature = string.Empty;

    [ObservableProperty]
    private string humidity = string.Empty;

    [ObservableProperty]
    private string gas = string.Empty;
    #endregion

    readonly HttpClient client = new HttpClient();
    readonly IDataRepository dataRepository;

    // O token do Blynk
    const string token = "6ju-aH1fAZD97gvIwbui6qceTEWIF0Fm";

    private readonly ISerialCommunicationService serialCommunicationService;
    private readonly System.Timers.Timer timer;
    private double _humidity;
    private double _temperature;
    private double _gas;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService, IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
        this.serialCommunicationService = serialCommunicationService;
        InitializeDataReceived();
        timer = new()
        {
            Interval = 30000
        };
        timer.Elapsed += Timer_Elapsed;
    }

    private async void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        await dataRepository.Create(new Data { Date=Date,Gas=_gas, Humidity=_humidity, Location=Location, Temperature=_temperature, Time=Time, Timestamp=DateTime.Now });
    }

    private void InitializeDataReceived()
    {
        this.serialCommunicationService.DataReceived += OnDataReceived;
    }

    private async void OnDataReceived(object? sender, string data)
    {
        try
        {
            ResponseData? responseData = JsonSerializer.Deserialize<ResponseData>(data);

            if (responseData != null)
            {
                Location = responseData.location;
                Temperature = $"{responseData.temperature} ºC";
                Date = responseData.date;
                Time = responseData.time;
                Humidity = $"{responseData.humidity}%";
                Gas = $"{responseData.gas}%";

                _temperature = responseData.temperature;
                _humidity = responseData.humidity;
                _gas = responseData.gas;

                string pinoVirtual1 = "v0";
                string pinoVirtual2 = "v1";
                string pinoVirtual3 = "v2";
                string pinoVirtual4 = "v3";
                string pinoVirtual5 = "v4";

                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual1}={responseData.humidity}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual2}={responseData.temperature}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual3}={responseData.location}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual4}={responseData.date}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual5}={responseData.gas}");
            }
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}