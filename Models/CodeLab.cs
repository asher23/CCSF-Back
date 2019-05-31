using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreshmanCSForum.API.Models
{
  public class CodeLab
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; }

    [BsonElement("LeadId")]
    public string LeadId { get; set; }

    [BsonElement("Chat")]
    public Chat Chat { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }


    [BsonElement("Goal")]
    public string Goal { get; set; }

    [BsonElement("LookingFor")]
    public string LookingFor { get; set; }

    [BsonElement("Joining")]
    public string Joining { get; set; }

    [BsonElement("MemberIds")]
    public List<string> MemberIds { get; set; }

    [BsonElement("CommentIds")]
    public List<string> CommentIds { get; set; }

    [BsonElement("Photos")]
    public List<Photo> Photos { get; set; }

    [BsonElement("Applications")]
    public List<Application> Applications { get; set; }
  }
}