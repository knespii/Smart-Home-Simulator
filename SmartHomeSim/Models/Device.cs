using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHomeSim.Models;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Light), typeof(Thermostat))]
[BsonIgnoreExtraElements]
public abstract class Device
{
    [BsonId]
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string Type { get; set; }
    public bool IsOn { get; set; }
    public abstract double GetConsumption();
    
    [BsonElement("roomId")]
    public ObjectId RoomId { get; set; }
}
[BsonIgnoreExtraElements]
public class Light : Device
{
    public  int Brightness { get; set; }
    public override double GetConsumption()
    {
        if (!IsOn) return 0;
        return (Brightness / 100.0) * 10;
    }
}

public class Thermostat : Device
{
    public double TargetTemperature { get; set; }
    public override double GetConsumption()
    {
        if (!IsOn) return 0;
        return 1500;
    }
}
