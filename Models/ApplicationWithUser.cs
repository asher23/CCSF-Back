using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class ApplicationWithUser
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Applicant")]
    public User Applicant { get; set; }

    [BsonElement("Message")]
    public string Message { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; }
  }
}