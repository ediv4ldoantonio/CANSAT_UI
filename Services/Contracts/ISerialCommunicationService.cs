namespace CANSAT_UI.Services.Contracts;

/// <summary>
/// Represents a service for serial communication.
/// </summary>
public interface ISerialCommunicationService
{
    /// <summary>
    /// Event raised when data is received from the serial port.
    /// </summary>
    event EventHandler<string> DataReceived;

    /// <summary>
    /// Gets a value indicating whether the service is currently connected to the serial port.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Opens the serial port connection.
    /// </summary>
    void OpenConnection(string portName, int baudRate);

    /// <summary>
    /// Sends data through the serial port.
    /// </summary>
    /// <param name="data">The data to send.</param>
    void SendData(string data);

    /// <summary>
    /// Closes the serial port connection.
    /// </summary>
    void CloseConnection();

    /// <summary>
    /// Gets the available COM ports.
    /// </summary>
    /// <returns>A list of available COM ports.</returns>
    IEnumerable<string> GetAvailablePorts();

    /// <summary>
    /// Release the resource.
    /// </summary>
    void Dispose();

    /// <summary>
    /// Subscribes to the DataReceived event.
    /// </summary>
    /// <param name="handler">The event handler to subscribe.</param>
    void SubscribeToDataReceivedEvent(EventHandler<string> handler);

    /// <summary>
    /// Unsubscribes from the DataReceived event.
    /// </summary>
    /// <param name="handler">The event handler to unsubscribe.</param>
    void UnsubscribeFromDataReceivedEvent(EventHandler<string> handler);
}
