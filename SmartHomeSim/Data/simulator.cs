using System;
using System.Collections.Generic;
using System.Linq;
using SmartHomeSim.Models;

namespace SmartHomeSim.Data;

public enum HomeMode
{
    Normal,
    Night,
    Vacation
}

public class Simulator
{
    public DateTime currentTime { get; set; }
    public double HomeConsumptionkWh { get; set; }
    public List<Automation> Automations { get; set; } = new List<Automation>();
    
    public HomeMode CurrentMode { get; set; } = HomeMode.Normal;

    public Simulator()
    {
        currentTime = DateTime.Now;
        HomeConsumptionkWh = 0;
    }

    public void TimeStep(int hours, List<Device> allDevices)
    {
        for (int i = 0; i < hours; i++)
        {
            currentTime = currentTime.AddHours(1);

            ApplyModeLogic(allDevices);

            if (CurrentMode != HomeMode.Vacation)
            {
                ExecuteAutomations(allDevices);
            }

            CalculateStepConsumption(allDevices);
        }
    }

    private void ApplyModeLogic(List<Device> allDevices)
    {
        switch (CurrentMode)
        {
            case HomeMode.Night:
                foreach (var light in allDevices.OfType<Light>())
                {
                    light.IsOn = false;
                }
                break;

            case HomeMode.Vacation:
                foreach (var dev in allDevices)
                {
                    if (dev is Thermostat thermo)
                    {
                        thermo.IsOn = true;  
                    }
                    else
                    {
                        dev.IsOn = false;
                    }
                }
                break;
        }
    }

    private void ExecuteAutomations(List<Device> allDevices)
    {
        foreach (var auto in Automations)
        {
            if (auto.Hour == currentTime.Hour)
            {
                var device = allDevices.Find(d => d.Name.Equals(auto.DeviceName, StringComparison.OrdinalIgnoreCase));
                if (device != null)
                {
                    device.IsOn = auto.SetIsOn;
                }
            }
        }
    }

    private void CalculateStepConsumption(List<Device> allDevices)
    {
        double currentPowerW = 0;
        foreach (var dev in allDevices)
        {
            double cons = dev.GetConsumption();
            if (cons > 0)
            {
                Console.WriteLine($"[SIM] {currentTime:HH:mm} - Režim: {CurrentMode} - {dev.Name}: {cons}W");
            }
            currentPowerW += cons;
        }
        HomeConsumptionkWh += currentPowerW / 1000.0;
    }
}