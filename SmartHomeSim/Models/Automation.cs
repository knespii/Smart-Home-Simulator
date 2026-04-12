namespace SmartHomeSim.Models;

public class Automation
{
    public int Hour {get; set; }
    public string DeviceName { get; set; } = "";
    public bool SetIsOn { get; set; }
}