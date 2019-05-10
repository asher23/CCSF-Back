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

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("LookingFor")]
    public string LookingFor { get; set; }

    [BsonElement("FinalGoalURL")]
    public string FinalGoalURL { get; set; }

    [BsonElement("MemberIds")]
    public List<string> MemberIds { get; set; }
  }
}