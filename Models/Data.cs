namespace CANSAT_UI.Models;

public class Data
{
    public required string Location { get; set; }
    public required string Date { get; set; }
    public required string Time { get; set; }
    public required double Temperature { get; set; }
    public required double Humidity { get; set; }
    public DateTime Timestamp { get; set; }
}
