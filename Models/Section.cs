using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class Section
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("Title")]
    public string Title { get; set; }

    [BsonElement("Details")]
    public string Details { get; set; }

    [BsonElement("URLs")]
    public string URLs { get; set; }

    [BsonElement("RankNumber")]
    public int RankNumber { get; set; }
  }
}