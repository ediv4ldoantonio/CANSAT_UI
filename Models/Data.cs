namespace CANSAT_UI.Models;

public class Data
{
    public required string Current { get; set; }
    public required string Power { get; set; }
    public required string Energy { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public required string Charge { get; set; }
}
