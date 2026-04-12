using System;
using SmartHomeSim.Models;
using SmartHomeSim.Data;
using MongoDB.Bson;

namespace SmartHomeSim.UI;

public class MenuHandler
{
    private readonly MongoService _db;
    private User? _loggedUser;
    private Simulator _sim = new Simulator();   
    public MenuHandler(MongoService db)
    {
        _db = db;
    }

    public void RunLogin()
    {
        while (_loggedUser == null)
        {
            Console.Clear();
            Console.WriteLine("Přihlašovací jméno:");
            string username = Console.ReadLine() ?? "";

            Console.WriteLine("Heslo:");
            string password = Console.ReadLine() ?? "";

            _loggedUser = _db.Login(username, password);

            if (_loggedUser == null)
            {
                Console.WriteLine("špatné přihlašovací údaje");
                Console.ReadKey();
            }
        }
        
        Console.Clear();
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        bool run = true;
        while (run)
        {
            Console.Clear();
            Console.WriteLine($"---logged in as: {_loggedUser?.Username}---");
            Console.WriteLine("1 - Vypsat místnosti");
            Console.WriteLine("2 - Přidat místnost");
            Console.WriteLine("3 - zpočítat zpotřebu");
            Console.WriteLine("4 - nastavit pravidla");
            Console.WriteLine("5 - nastavit režim");
            Console.WriteLine("0 - Odhlásit se");
            
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    var rooms = _db.GetRooms();

                    if (rooms.Count == 0)
                    {
                        Console.WriteLine("nemáte vytvořené žádné místnosti");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("--- Seznam místností ---");
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {rooms[i].Name}");
                    }
                    Console.WriteLine("0. Zpět");

                    Console.WriteLine("\n Vyberte místnost pro správu:");
                    string input = Console.ReadLine() ?? "";
                    if (int.TryParse(input, out int index) && index > 0 && index <= rooms.Count)
                    {
                        ShowRoomDetail(rooms[index - 1]);
                    }
                    break;
                    
                case "2":
                    Console.Clear();
                    Console.WriteLine("Zadejte název nové místnosti:");
                    string newRoomName = Console.ReadLine() ?? "";
                    
                    if (string.IsNullOrWhiteSpace(newRoomName))
                    {
                        Console.WriteLine("Název nesmí být prázdný!");
                    }
                    else
                    {
                        _db.CreateRoom(newRoomName);
                        Console.WriteLine($"\nMístnost '{newRoomName}' byla úspěšně uložena.");
                        Console.ReadKey();
                    }
                    break;
                
                case "3":
                    Console.Clear();
                    var allDevices = _db.GetAllDevices();
                    _sim.TimeStep(1, allDevices);

                    if (allDevices.Count == 0)
                    {
                        Console.WriteLine("V domě nejsou žádná zařízení");
                    }
                    else
                    {
                        _sim.TimeStep(1, allDevices);
                    }

                    foreach (var dev in allDevices)
                    {
                        _db.UpdateDevice(dev);
                    }
                    
                    Console.WriteLine("--- Simulace času ---");
                    Console.WriteLine($"Čas posunut o 1 hodinu.");
                    Console.WriteLine($"Aktuální simulační čas: {_sim.currentTime}");
                    Console.WriteLine($"Celková spotřeba domu: {_sim.HomeConsumptionkWh:F2} kWh");
                    Console.WriteLine("\nStiskněte libovolnou klávesu...");
                    Console.ReadKey();
                    break;
                
                case "4":
                    Console.Clear();
                    Console.WriteLine("--- Nastavení nové automatizace ---");
                    Console.Write("V kolik hodin? (0-23): ");
                    int hour = int.Parse(Console.ReadLine());
    
                    Console.Write("Název zařízení: ");
                    string name = Console.ReadLine();
    
                    Console.Write("Zapnout (1) nebo Vypnout (0)? ");
                    bool state = Console.ReadLine() == "1";

                    var newAuto = new Automation { Hour = hour, DeviceName = name, SetIsOn = state };
    
                    _sim.Automations.Add(newAuto);
                    
                    Console.WriteLine("Automatizace nastavena!");
                    Console.ReadKey();
                    break;
                
                case "5":
                    Console.WriteLine("---Zvolte režim---");
                    Console.WriteLine("1.Normalní");
                    Console.WriteLine("2.Noc");
                    Console.WriteLine("3.Dovolená");
                    string volba = Console.ReadLine();
                    _sim.CurrentMode = volba switch 
                    {
                        "2" => HomeMode.Night,
                        "3" => HomeMode.Vacation,
                        _ => HomeMode.Normal 
                    };
                    break;
                
                case "0":
                    run = false;
                    _loggedUser = null;
                    break;
            }
        }
    }

    private void ShowRoomDetail(Room room)
    {
        bool inRoom = true;
        while (inRoom)
        {
            Console.Clear();
            Console.WriteLine($"--- Správa místnosti: {room.Name} ---");
            
            var devices = _db.GetDevicesInRoom(room.Id);

            if (devices.Count == 0)
            {
                Console.WriteLine("V této místnosti nejsou žádná zařízení.");
            }
            else
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    var d = devices[i];
                    string status = d.IsOn ? "ZAPNUTO" : "VYPNUTO";
                    Console.WriteLine($"{i + 1}. [{d.Type}] {d.Name} - {status}");
                }
            }

            Console.WriteLine("\n[A] Přidat zařízení | [D] Smazat zařízení | [U] Ovládat zařízení | [0] Zpět");
            string choice = Console.ReadLine()?.ToLower() ?? "";

            if (choice == "0")
            {
                inRoom = false;
            }
            else if (choice == "a")
            {
                AddDeviceMenu(room.Id);
            }
            else if (choice == "d")
            {
                DeleteDeviceMenu(devices);
            }
            else if (choice =="u")
            {
                Console.WriteLine("Zadej číslo zařízení pro ovládání:");
                string deviceInput = Console.ReadLine() ?? "";
                
                if (int.TryParse(deviceInput, out int deviceIdx) && deviceIdx > 0 && deviceIdx <= devices.Count)
                {
                    HandleDeviceControl(devices[deviceIdx - 1]);
                }
                else
                {
                    Console.WriteLine("Neplatné číslo zařízení!");
                    Console.ReadKey();
                }
            }
        }
    }

    private void HandleDeviceControl(Device dev)
    {
        bool controlling = true;
        while (controlling)
        {
            Console.Clear();
            Console.WriteLine($"--- Ovládání: {dev.Name} ({dev.Type}) ---");
            Console.WriteLine($"Stav: {(dev.IsOn ? "ZAPNUTO" : "VYPNUTO")}");

            if (dev is Thermostat t)
            {
                Console.WriteLine($"Cílová teplota: {t.TargetTemperature}°C");
            }
            else if (dev is Light l)
            {
                Console.WriteLine($"Jas: {l.Brightness}%");
            }

            Console.WriteLine("\n1 - Přepnout ZAP/VYP");
            if (dev is Thermostat) Console.WriteLine("2 - Nastavit teplotu");
            if (dev is Light) Console.WriteLine("2 - Nastavit jas");
            Console.WriteLine("0 - Zpět");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                dev.IsOn = !dev.IsOn;
            }
            else if (choice == "2")
            {
                if (dev is Thermostat thermo)
                {
                    Console.Write("Zadejte novou teplotu (5-30): ");
                    if (int.TryParse(Console.ReadLine(), out int temp) && temp >=5 && temp <=30 && thermo.IsOn)
                    {thermo.TargetTemperature = temp;}
                else
                {
                    Console.WriteLine("Zadali jste špatné hodnoty");
                    Console.ReadKey();
                }
                }
                else if (dev is Light light)
                {
                    Console.Write("Zadejte jas (0-100): ");
                    if (int.TryParse(Console.ReadLine(), out int bright) && bright >0 && bright <=100 && light.IsOn) 
                    {light.Brightness = bright;}
                    else
                    {
                        Console.WriteLine("Zadali jste špatné hodnoty");
                        Console.ReadKey();
                    }
                }
            }
            else if (choice == "0")
            {
                controlling = false;
            }

            _db.UpdateDevice(dev);
        }
    }

    private void AddDeviceMenu(ObjectId roomId)
    {
        Console.Clear();
        Console.WriteLine("Vyberte typ: 1. Světlo | 2. Thermostat");
        string typeChoice = Console.ReadLine() ?? "";
        
        Console.Write("Název zařízení: ");
        string name = Console.ReadLine() ?? "";

        Device? newDev = typeChoice switch
        {
            "1" => new Light { Name = name, Type = "Light", RoomId = roomId, Brightness = 100 },
            "2" => new Thermostat { Name = name, Type = "Thermostat", RoomId = roomId, TargetTemperature = 22.0 },
            _ => null
        };

        if (newDev != null)
        {
            _db.AddDevice(newDev);
        }
    }

    private void DeleteDeviceMenu(List<Device> devices)
    {
        Console.Write("Zadejte číslo zařízení ke smazání: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= devices.Count)
        {
            _db.DeleteDevice(devices[idx - 1].Id);
        }
    }
}