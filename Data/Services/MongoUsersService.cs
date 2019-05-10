using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Helpers;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Data
{
  public class MongoUsersService : IUsersService
  {
    private readonly IMongoCollection<User> _users;

    public MongoUsersService(MyMongoDatabase database)
    {
      _users = database.Users;
    }

    public async Task Delete(string id)
    {
      await _users.DeleteOneAsync(Builders<User>.Filter.Eq(x => x.Id, id));
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      return await _users.Find(Builders<User>.Filter.Empty).ToListAsync();
    }

    public Task<User> GetOne(string id)
    {
      return _users.Find(Builders<User>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
    }

    public async Task<User> Update(string id, User user)
    {
      User result = await _users.FindOneAndUpdateAsync(
        Builders<User>.Filter.Eq(x => x.Id, id),
        UpdateBuilders.getUpdate(user),
        new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After }
      );
      return result;
    }

    public async Task<bool> AddCodeLab(string codeLabId, string userId)
    {
      var updateResult = await _users.UpdateOneAsync(
        Builders<User>.Filter.Eq(x => x.Id, userId),
        Builders<User>.Update.Push(x => x.CodeLabIds, codeLabId)
        );
      return updateResult.IsAcknowledged;
    }
  }
}