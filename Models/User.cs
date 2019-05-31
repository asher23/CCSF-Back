using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("FirstName")]
    public string FirstName { get; set; }

    [BsonElement("LastName")]
    public string LastName { get; set; }

    [BsonElement("Username")]
    public string Username { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }


    [BsonElement("PasswordHash")]
    public byte[] PasswordHash { get; set; }

    [BsonElement("PasswordSalt")]
    public byte[] PasswordSalt { get; set; }

    [BsonElement("Introduction")]
    public string Introduction { get; set; }

    [BsonElement("GuideIds")]
    public List<string> GuideIds { get; set; }

    [BsonElement("CommentIds")]
    public List<string> CommentIds { get; set; }

    [BsonElement("CodeLabIds")]
    public List<string> CodeLabIds { get; set; }

    [BsonElement("Photo")]
    public Photo Photo { get; set; }
  }
}