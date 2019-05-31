using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class ChatMessage
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("SenderId")]
    public string SenderId { get; set; }

    [BsonElement("SenderUsername")]
    public string SenderUsername { get; set; }

    [BsonElement("Message")]
    public string Message { get; set; }
  }
}