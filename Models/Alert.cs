namespace CANSAT_UI.Models;

public class Alert
{
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
