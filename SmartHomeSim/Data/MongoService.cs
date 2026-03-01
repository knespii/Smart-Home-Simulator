using MongoDB.Driver;
using SmartHomeSim.Models;
using Microsoft.Extensions.Configuration;
namespace SmartHomeSim.Data;

public class MongoService
{
    private readonly IMongoDatabase _database;

    public MongoService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("smarthomesim");
    }

    public User? Login(string username, string password)
    {
        var collection = _database.GetCollection<User>("users");
        var User = collection.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
        return User;
    }

    public List<User> GetUsers()
    {
        var collection = _database.GetCollection<User>("users");
        return collection.Find(_ => true).ToList();
    } 

    public List<Room> GetRooms()
    {
        var collection = _database.GetCollection<Room>("Rooms");
        return collection.Find(_ => true).ToList();
    }

    public void CreateRoom(string roomName)
    {
        var collection = _database.GetCollection<Room>("Rooms");
        var newRoom = new Room { Name = roomName };
        collection.InsertOne(newRoom);
    }

}