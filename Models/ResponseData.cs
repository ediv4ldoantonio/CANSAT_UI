namespace CANSAT_UI.Models;

public class ResponseData
{
    public required string[] correntes { get; set; }
    public required string[] energias { get; set; }
    public required string[] potencias { get; set; }
    public required bool[] estados { get; set; }
}
