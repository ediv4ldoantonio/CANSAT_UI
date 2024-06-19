﻿using CANSAT_UI.Controls;
using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;


namespace CANSAT_UI.ViewModels;


public partial class DashboardViewModel : ViewModelBase
{
    #region ObservableProperties
    [ObservableProperty]
    private string[] correntes;

    [ObservableProperty]
    private string[] potencias;

    [ObservableProperty]
    private string[] energias;

    [ObservableProperty]
    private bool[] estados;

    #endregion
    readonly HttpClient client;
    readonly MySQLDataRepository dataRepository;

    // O token do Blynk
    const string token = "kSAjm1XA1f2zwP08H-hDikqDyL8R2r_B";

    private readonly ISerialCommunicationService serialCommunicationService;
    private readonly Timer timer;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService, MySQLDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
        this.client = new HttpClient();

        this.correntes = new string[3];
        this.energias = new string[3];
        this.potencias = new string[3];

        this.estados = new bool[2];

        this.serialCommunicationService = serialCommunicationService;

        timer = new Timer
        {
            Interval = 30000
        };

        timer.Elapsed += Timer_Elapsed;
        timer.Start();
        InitializeDataReceived();
    }

    private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            for (int i = 0; i < 3; i++)
            {
                string carga = i == 0 ? "tomada" : i == 1 ? "lampada a" : "lampada b";

                await dataRepository.Create(new Data()
                {
                    Charge = carga,
                    Current = Correntes[i],
                    Power = Potencias[i],
                    Energy = Energias[i]
                });
            }
        }
        catch
        {

        }
    }

    private void InitializeDataReceived()
    {
        serialCommunicationService.DataReceived += OnDataReceived;
    }

    private async void OnDataReceived(object? sender, string data)
    {
        try
        {
            ResponseData? responseData = JsonSerializer.Deserialize<Models.ResponseData>(data);

            if (responseData != null)
            {
                for (int i = 0; i < responseData.potencias.Length; i++)
                    Potencias[i] = $"{responseData.potencias[i]} W";

                for (int i = 0; i < responseData.correntes.Length; i++)
                    Correntes[i] = $"{responseData.correntes[i]} A";

                for (int i = 0; i < responseData.energias.Length; i++)
                    Energias[i] = $"{responseData.energias[i]} J";

                for (int i = 0; i < responseData.estados.Length; i++)
                    Estados[i] = responseData.estados[i];

                OnPropertyChanged(nameof(Potencias));
                OnPropertyChanged(nameof(Energias));
                OnPropertyChanged(nameof(Correntes));
                OnPropertyChanged(nameof(Estados));

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
        string pinoVirtual4 = "v3";
        string pinoVirtual5 = "v4";

        string tomada = $"{Correntes[0]}, {Potencias[0]}, {Energias[0]}";
        string lampadaA = $"{Correntes[1]}, {Potencias[1]}, {Energias[1]}";
        string lampadaB = $"{Correntes[2]}, {Potencias[2]}, {Energias[2]}";

        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual1}={tomada}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual2}={lampadaA}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual3}={lampadaB}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual4}={Estados[0]}");
        await client.GetAsync($"https://blynk.cloud/external/api/update?token={token}&{pinoVirtual5}={Estados[1]}");
    }

    [RelayCommand]
    public void ChangeState(object sender)
    {
        try
        {
            ChargeControl control = (ChargeControl)sender;

            bool value = control.IsTriggered;
            string charge = control.Tag.ToString()!;

            if (charge == "A")
                serialCommunicationService.SendData(value ? "1" : "2");
            else if (charge == "B")
                serialCommunicationService.SendData(value ? "3" : "4");
        }
        catch 
        {

        }
    }
}