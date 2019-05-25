using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class GuideWithComments
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; }

    [BsonElement("Sections")]
    public List<Section> Sections { get; set; }

    [BsonElement("CreatorId")]
    public string CreatorId { get; set; }

    [BsonElement("Comments")]
    public IEnumerable<Comment> Comments { get; set; }

    [BsonElement]
    public string Description { get; set; }
  }
}