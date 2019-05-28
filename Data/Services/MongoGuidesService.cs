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

    public async Task<IEnumerable<Guide>> GetAllForProfile(string creatorId)
    {
      return await _guides.Find(Builders<Guide>.Filter.Eq(x => x.CreatorId, creatorId)).ToListAsync();
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
        ).Project(p => new { p.Id, p.Title, p.Sections, p.CreatorId, Comments = p.Comments, p.Description, p.Photos })
        .ToListAsync();

      GuideWithComments guideWithComments = new GuideWithComments
      {
        Id = query[0].Id,
        Title = query[0].Title,
        Sections = query[0].Sections,
        CreatorId = query[0].CreatorId,
        Comments = query[0].Comments,
        Description = query[0].Description,
        Photos = query[0].Photos
      };


      return guideWithComments;
    }

    public async Task<string> GetCreatorId(string id)
    {
      Guide guide = await _guides.Find(Builders<Guide>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();

      return guide.CreatorId;
    }

    public Task Create(Guide guide)
    {
      return _guides.InsertOneAsync(guide);
    }

    public async Task<Guide> Update(string id, Guide guide)
    {
      var updateList = new List<UpdateDefinition<Guide>>();
      // var update = Builders<Guide>.Update.Set("Title", guide.Title);
      if (guide.Photos != null) updateList.Add(Builders<Guide>.Update.Set("Photos", guide.Photos));
      if (guide.Sections != null) updateList.Add(Builders<Guide>.Update.Set("Sections", guide.Sections));
      if (guide.Title != null) updateList.Add(Builders<Guide>.Update.Set("Title", guide.Title));
      if (guide.Description != null) updateList.Add(Builders<Guide>.Update.Set("Description", guide.Description));
      if (guide.CreatorId != null) updateList.Add(Builders<Guide>.Update.Set("CreatorId", guide.CreatorId));
      var finalUpdate = Builders<Guide>.Update.Combine(updateList);
      var result = await _guides.FindOneAndUpdateAsync(
        Builders<Guide>.Filter.Eq(x => x.Id, id),
        // UpdateBuilders.getUpdate(guide),
        finalUpdate,
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