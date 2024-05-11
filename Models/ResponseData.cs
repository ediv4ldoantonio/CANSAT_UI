namespace CANSAT_UI.Models;

public class ResponseData
{
    public required string location { get; set; }
    public required string date { get; set; }
    public required string time { get; set; }
    public required double temperature { get; set; }
    public required double humidity { get; set; }
}
