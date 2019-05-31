using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class Application
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("ApplicantId")]
    public string ApplicantId { get; set; }

    [BsonElement("Message")]
    public string Message { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; }
  }
}