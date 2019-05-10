using FreshmanCSForum.API.Models;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Data
{
  public class MyMongoDatabase
  {
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    public MyMongoDatabase(string connectionString)
    {
      _mongoClient = new MongoClient(connectionString);
      _database = _mongoClient.GetDatabase("FreshmanCSForumDb");
      Users = _database.GetCollection<User>("Users");
      Guides = _database.GetCollection<Guide>("Guides");
      CodeLabs = _database.GetCollection<CodeLab>("CodeLabs");
      Comments = _database.GetCollection<Comment>("Comments");
    }

    public IMongoCollection<User> Users { get; set; }

    public IMongoCollection<Guide> Guides { get; set; }

    public IMongoCollection<CodeLab> CodeLabs { get; set; }

    public IMongoCollection<Comment> Comments { get; set; }
  }
}