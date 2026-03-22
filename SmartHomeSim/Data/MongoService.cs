using MongoDB.Driver;
using SmartHomeSim.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
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
        var user = collection.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
        return user;
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

    public void AddDevice(Device device)
    {
        var collection = _database.GetCollection<Device>("devices");
        collection.InsertOne(device);
    }
    
    public void ToggleDevice(ObjectId deviceId, bool newState)
    {
        var collection = _database.GetCollection<Device>("devices");
        var filter = Builders<Device>.Filter.Eq(d => d.Id, deviceId);
        var update = Builders<Device>.Update.Set(d => d.IsOn, newState);
        collection.UpdateOne(filter, update);
    }
    
    public void DeleteDevice(ObjectId deviceId)
    {
        var collection = _database.GetCollection<Device>("devices");
        collection.DeleteOne(d => d.Id == deviceId);
    }

    public List<Device> GetDevicesInRoom(ObjectId roomId)
    {
        var collection = _database.GetCollection<Device>("devices");
        return collection.Find(d => d.RoomId == roomId).ToList();
    }
}