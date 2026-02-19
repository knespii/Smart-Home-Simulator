namespace SmartHomeSim.Models;

public class Device
{
    public required string Name { get; set; }
    public bool IsOn { get; set; }
    public double EnergyConsumption { get; set; }
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

