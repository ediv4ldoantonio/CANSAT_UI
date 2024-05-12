﻿using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using System.Net.Http;
using System.Text.Json;

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
    #endregion

    readonly HttpClient client = new HttpClient();
    readonly IDataRepository dataRepository;

    [ObservableProperty]
    private ChartValues<double> temperatureValues;

    // O token do Blynk
    const string token = "6ju-aH1fAZD97gvIwbui6qceTEWIF0Fm";

    private readonly ISerialCommunicationService serialCommunicationService;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService, IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
        this.serialCommunicationService = serialCommunicationService;
        InitializeDataReceived();
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

                TemperatureValues.Add(responseData.humidity);
                OnPropertyChanged(nameof(TemperatureValues));

                string pinoVirtual1 = "v0";
                string pinoVirtual2 = "v1";
                string pinoVirtual3 = "v2";

                await dataRepository.Create(new Data { Date = Date, Humidity = responseData.humidity, Location = Location, Temperature = responseData.temperature, Time = Time, Timestamp = DateTime.Now });

                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual1}={responseData.humidity}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual2}={responseData.temperature}");
                await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual3}={responseData.location}");
            }
        }
        catch
        {

        }
    }
}