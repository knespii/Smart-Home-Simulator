using System;
using SmartHomeSim.Models;
using SmartHomeSim.Data;
using MongoDB.Bson;

namespace SmartHomeSim.UI;

public class MenuHandler
{
    private readonly MongoService _db;
    private User? _loggedUser;
    
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
            String username = Console.ReadLine() ?? "";

            Console.WriteLine("Heslo:");
            String password = Console.ReadLine() ?? "";

            _loggedUser = _db.Login(username ?? "", password ?? "");

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
            Console.WriteLine("0 - Odhlásit se");
            
            string choice = Console.ReadLine()?? "";

            switch (choice)
            {
                case("1"):
                    Console.Clear();
                    var rooms = _db.GetRooms();

                    if (rooms.Count == 0)
                    {
                        Console.WriteLine("nemáte vytvořené žádné místnosti");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
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
                    }
                    break;
                    
                case("2"):
                    Console.Clear();
                    Console.WriteLine("Zadejte název nové místnosti:");
                    string newRoomName = Console.ReadLine()?? "";
                    
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
                
                case("0"):
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

            Console.WriteLine("\n[A] Přidat zařízení | [S] Smazat zařízení | [0] Zpět");
            string choice = Console.ReadLine()?.ToLower() ?? "";

            if (choice == "0")
            {
                inRoom = false;
            }
            else if (choice == "a")
            {
                AddDeviceMenu(room.Id);
            }
            else if (choice == "s")
            {
                DeleteDeviceMenu(devices);
            }
        }
    }

    private void AddDeviceMenu(ObjectId roomId)
    {
        Console.Clear();
        Console.WriteLine("Vyberte typ: 1. Světlo | 2. Termostat | 3. Senzor");
        string typeChoice = Console.ReadLine() ?? "";
        
        Console.Write("Název zařízení: ");
        string name = Console.ReadLine() ?? "";

        Device? newDev = typeChoice switch
        {
            "1" => new Light { Name = name, Type = "Light", RoomId = roomId },
            "2" => new Thermostat { Name = name, Type = "Thermostat", RoomId = roomId },
            "3" => new MotionSensor { Name = name, Type = "MotionSensor", RoomId = roomId },
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