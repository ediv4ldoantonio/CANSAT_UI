namespace CANSAT_UI.Models;

public class Data
{
    public required double Level { get; set; }
    public required int Number { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
