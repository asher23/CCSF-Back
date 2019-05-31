using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class Chat
  {

    public Chat()
    {
      Messages = new List<ChatMessage>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Messages")]
    public List<ChatMessage> Messages { get; set; }

  }
}