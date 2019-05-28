using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class Guide
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Sections")]
    public List<Section> Sections { get; set; }

    [BsonElement("Photos")]
    public List<Photo> Photos { get; set; }

    [BsonElement("CreatorId")]
    public string CreatorId { get; set; }
  }
}