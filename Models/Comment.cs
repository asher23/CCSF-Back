using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class Comment
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Details")]
    public string Details { get; set; }

    [BsonElement("UserId")]
    public string UserId { get; set; }

    [BsonElement("Username")]
    public string Username { get; set; }

    [BsonElement("PostId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; }

    [BsonElement("IsFor")]
    public string IsFor { get; set; }
  }
}