using CANSAT_UI.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace CANSAT_UI.ViewModels;

public partial class ConnectionViewModel : ViewModelBase
{
    public IEnumerable<string> AvailablePorts => serialCommunicationService.GetAvailablePorts();

    public IEnumerable<string> BaudRatesOptions => ["4800", "9600", "14400", "19200"];

    private readonly ISerialCommunicationService serialCommunicationService;

    [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
    [ObservableProperty]
    private bool isConnected;

    [ObservableProperty]
    private int baudRate;

    [ObservableProperty]
    private string? portName;


    public ConnectionViewModel(ISerialCommunicationService serialCommunicationService)
    {
        this.serialCommunicationService = serialCommunicationService;
    }


    [RelayCommand(CanExecute = nameof(CanConnect))]
    public async Task ConnectAsync()
    {
        if (string.IsNullOrEmpty(PortName))
        {
            MessageBox.Show("Por favor, selecione a porta", "Erro!");
            return;
        }
        else if (BaudRate == 0)
        {
            MessageBox.Show("Por favor, selecione o Baud Rate", "Erro!");
            return;
        }

        IsBusy = true;

        try
        {
            await Task.Run(() => serialCommunicationService.OpenConnection(PortName, BaudRate));

            IsConnected = true;
        }
        catch (Exception)
        {
            MessageBox.Show("Oops! Falha ao conectar", "Erro!");
        }
        finally
        {
            IsBusy = false;
        }

    }

    [RelayCommand]
    public async Task DisconnectAsync()
    {
        IsBusy = true;

        try
        {
            await Task.Run(() => serialCommunicationService.CloseConnection());

            IsConnected = false;
        }
        catch (Exception)
        {
            MessageBox.Show("Oops! Falha ao conectar", "Erro!");
        }
        finally
        {
            IsBusy = false;
        }

    }

    private bool CanConnect() => !IsConnected;
}
