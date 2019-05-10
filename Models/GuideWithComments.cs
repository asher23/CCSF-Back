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

    [BsonElement("SubSections")]
    public List<SubSection> SubSections { get; set; }

    [BsonElement("UserId")]
    public string UserId { get; set; }

    [BsonElement("Comments")]
    public IEnumerable<Comment> Comments { get; set; }
  }
}