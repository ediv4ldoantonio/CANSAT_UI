using CANSAT_UI.Services.Contracts;
using System.IO.Ports;


namespace CANSAT_UI.Services;

/// <summary>
/// Represents a service for serial communication.
/// </summary>
public class SerialCommunicationService : ISerialCommunicationService
{
    /// <summary>
    /// Event raised when data is received from the serial port.
    /// </summary>
    public event EventHandler<string>? DataReceived;
    private readonly SerialPort serialPort;

    public bool IsConnected => serialPort.IsOpen;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommunicationService"/> class.
    /// </summary>
    /// <param name="serialPort">The serial port object instance.</param>
    public SerialCommunicationService(SerialPort serialPort)
    {
        this.serialPort = serialPort;
        this.serialPort.DataReceived += SerialPort_DataReceived;
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        // Read the data from the serial port and raise the event
        string data = serialPort.ReadLine();
        DataReceived?.Invoke(this, data);
    }

    /// <summary>
    /// Closes the serial port connection.
    /// </summary>
    public void CloseConnection()
    {
        serialPort.Close();
    }

    /// <summary>
    /// Opens the serial port connection.
    /// </summary>
    public void OpenConnection(string portName, int baudRate)
    {
        serialPort.PortName = portName;
        serialPort.BaudRate = baudRate;

        serialPort.Open();
    }

    /// <summary>
    /// Sends data through the serial port.
    /// </summary>
    /// <param name="data">The data to send.</param>
    public void SendData(string data)
    {
        serialPort.WriteLine(data);
    }

    /// <summary>
    /// Subscribes to the DataReceived event.
    /// </summary>
    /// <param name="handler">The event handler to subscribe.</param>
    public void SubscribeToDataReceivedEvent(EventHandler<string> handler)
    {
        DataReceived += handler;
    }

    /// <summary>
    /// Unsubscribes from the DataReceived event.
    /// </summary>
    /// <param name="handler">The event handler to unsubscribe.</param>
    public void UnsubscribeFromDataReceivedEvent(EventHandler<string> handler)
    {
        DataReceived -= handler;
    }

    public IEnumerable<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }


    /// <summary>
    /// Release all the resourses.
    /// </summary>
    public void Dispose()
    {
        serialPort.Dispose();
    }
}
