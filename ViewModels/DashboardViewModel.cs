using CANSAT_UI.Controls;
using CANSAT_UI.Models;
using CANSAT_UI.Repositories;
using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;


namespace CANSAT_UI.ViewModels;


public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool[] estados;

    readonly MySQLDataRepository dataRepository;


    private readonly ISerialCommunicationService serialCommunicationService;
    private readonly Timer timer;

    public DashboardViewModel(ISerialCommunicationService serialCommunicationService, MySQLDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;

        this.estados = new bool[3];

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
                string carga = i switch
                {
                    0 => "lampada a",
                    1 => "lampada b",
                    2 => "lampada c",
                    _ => throw new ArgumentOutOfRangeException(nameof(i), "Valor inesperado")
                };


                await dataRepository.Create(new Data()
                {
                    Charge = carga,
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

    private void OnDataReceived(object? sender, string data)
    {
        try
        {
            ResponseData? responseData = JsonSerializer.Deserialize<Models.ResponseData>(data);

            if (responseData != null)
            {
                switch(responseData.carga)
                {
                    case "a":
                        Estados[0] = responseData.estado;
                        break;
                    case "b":
                        Estados[1] = responseData.estado;
                        break;
                    case "c":
                        Estados[2] = responseData.estado;
                        break;
                }

                OnPropertyChanged(nameof(Estados));
            }
        }
        catch
        {

        }
    }

    [RelayCommand]
    public void ChangeState(object sender)
    {
        try
        {
            ChargeControl control = (ChargeControl)sender;

            bool value = control.IsTriggered;
            string charge = control.Tag.ToString()!;

            switch(charge)
            {
                case "A":
                    serialCommunicationService.SendData(value ? "A" : "a");
                    break;
                case "B":
                    serialCommunicationService.SendData(value ? "B" : "b");
                    break;
                case "C":
                    serialCommunicationService.SendData(value ? "C" : "c");
                    break;
            }
        }
        catch 
        {

        }
    }
}