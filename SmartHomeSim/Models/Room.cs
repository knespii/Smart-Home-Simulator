namespace SmartHomeSim.Models;

public class Room
{
    public MongoDB.Bson.ObjectId Id { get; set; }
    
    public string Name { get; set; }
    public int CurrentTemperature { get; set; }
    public bool IsOccupied { get; set; }

    public List<Device> Devices { get; set; } = new List<Device>();
}