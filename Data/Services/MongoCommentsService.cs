using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Helpers;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;
using FreshmanCSForum.API.Data.Interfaces;


namespace FreshmanCSForum.API.Data
{
  public class MongoCommentsService : ICommentsService
  {
    private readonly IMongoCollection<Comment> _comments;
    public MongoCommentsService(MyMongoDatabase database)
    {
      _comments = database.Comments;
    }
    public Task Create(Comment comment)
    {
      return _comments.InsertOneAsync(comment);
    }

    public Task Delete(string id)
    {
      return _comments.DeleteOneAsync(Builders<Comment>.Filter.Eq(x => x.Id, id));
    }

    public async Task<IEnumerable<Comment>> GetAllForPost(string postId)
    {
      return await _comments.Find(
          Builders<Comment>.Filter.Eq(x => x.PostId, postId)
      ).ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetAllForUser(string userId)
    {
      return await _comments.Find(
          Builders<Comment>.Filter.Eq(x => x.UserId, userId)
      ).ToListAsync();
    }

    public async Task<Comment> GetOne(string id)
    {
      return await _comments.Find(Builders<Comment>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
    }

    public async Task<Comment> Update(string id, Comment comment)
    {
      var result = await _comments.FindOneAndUpdateAsync(
        Builders<Comment>.Filter.Eq(x => x.Id, id),
        UpdateBuilders.getUpdate(comment),
        new FindOneAndUpdateOptions<Comment> { ReturnDocument = ReturnDocument.After }
      );
      return result;
    }
  }
}