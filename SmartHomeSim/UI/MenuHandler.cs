using System;
using SmartHomeSim.Models;
using SmartHomeSim.Data;
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
                    }
                    else
                    {
                        foreach (var Room in rooms)
                        {
                            Console.WriteLine($"-  {Room.Name}");
                        }
                    } 
                    Console.WriteLine("\n Pro vrácení stiskněte libovolnou klávesu");
                    Console.ReadKey();
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
                    }
                    break;
                
                case("0"):
                    run = false;
                    _loggedUser = null;
                    break;
            }
        }
    }

}