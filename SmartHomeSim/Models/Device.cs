using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHomeSim.Models;

public class Device
{
    [BsonId]
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string Type { get; set; }
    public bool IsOn { get; set; }
    public double Value { get; set; }
    
    [BsonElement("roomId")]
    public ObjectId RoomId { get; set; }
}

public class Light : Device
{
    public  int Brightness { get; set; }
}

public class Thermostat : Device
{
    public double TargetTemperature { get; set; }
}

public class MotionSensor : Device
{
    public bool MovementDetected { get; set; }
}

