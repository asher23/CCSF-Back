using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson;
using FreshmanCSForum.API.Helpers;
using Newtonsoft.Json.Linq;
using FreshmanCSForum.API.Data.Interfaces;

namespace FreshmanCSForum.API.Data
{
  public class MongoGuidesService : IGuidesService
  {
    private readonly IMongoCollection<Guide> _guides;
    private readonly IMongoCollection<Comment> _comments;
    public MongoGuidesService(MyMongoDatabase database)
    {
      _guides = database.Guides;
      _comments = database.Comments;
    }
    public Task Delete(string id)
    {
      return _guides.DeleteOneAsync(Builders<Guide>.Filter.Eq(x => x.Id, id));
    }

    public async Task<IEnumerable<Guide>> GetAll()
    {
      return await _guides.Find(Builders<Guide>.Filter.Empty).ToListAsync();
    }

    public async Task<GuideWithComments> GetOne(string id)
    {
      var query = await _guides.Aggregate()
        .Match(p => p.Id == id)
        .Lookup(
          foreignCollection: _comments,
          localField: g => g.Id,
          foreignField: f => f.PostId,
          @as: (GuideWithComments gwc) => gwc.Comments
        ).Project(p => new { p.Id, p.Title, p.SubSections, p.UserId, Comments = p.Comments })
        .ToListAsync();

      GuideWithComments guideWithComments = new GuideWithComments
      {
        Id = query[0].Id,
        Title = query[0].Title,
        SubSections = query[0].SubSections,
        UserId = query[0].UserId,
        Comments = query[0].Comments,
      };

      return guideWithComments;
    }

    public Task Create(Guide guide)
    {
      return _guides.InsertOneAsync(guide);
    }

    public async Task<Guide> Update(string id, Guide guide)
    {
      var result = await _guides.FindOneAndUpdateAsync(
        Builders<Guide>.Filter.Eq(x => x.Id, id),
        UpdateBuilders.getUpdate(guide),
        new FindOneAndUpdateOptions<Guide> { ReturnDocument = ReturnDocument.After }
      );
      return result;
    }

    public async Task<GuideWithComments> AddComment(Comment comment)
    {
      await _comments.InsertOneAsync(comment);
      return await GetOne(comment.PostId);

    }
  }
}


// Guide guide = await _guides.FindOneAndUpdateAsync(
//    Builders<Guide>.Filter.Eq(x => x.Id, comment.PostId),
//    Builders<Guide>.Update.AddToSet(x => x.CommentIds, comment.Id),
//    new FindOneAndUpdateOptions<Guide> { ReturnDocument = ReturnDocument.After }
// );

// BsonArray subpipeline = new BsonArray();
// subpipeline.Add(
//   new BsonDocument("$match", new BsonDocument(
//     "$expr", new BsonDocument(
//       "$eq", new BsonArray { "$$comment", "comment"}
//     )
//   ))
// );
// var lookup = new BsonDocument("$lookup",
//   new BsonDocument("from", "others")
//     .Add("let", new BsonDocument("entity", '$_id'))
//     .Add("pipeline", subpipeline)
//     .Add("as", "others")
// );

// //entity is comment
// //others is guide
// GuideWithComments newGuide = query1;
//   .AppendStage<EntityWithOthers>(lookup)
//   .Unwind<EntityWithOthers, EntityWithOther>(p => p.others)
//   .SortByDescending(p => p.others.name)
//   .ToList();

// Guide fullGuide = await _guides.Aggregate();
// // guide.Comments = comments;