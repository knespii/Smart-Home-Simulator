namespace SmartHomeSim.Models;

public class User
{
    public MongoDB.Bson.ObjectId Id { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}