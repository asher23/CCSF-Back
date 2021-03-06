using FreshmanCSForum.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Helpers
{
  public class UpdateBuilders
  {
    public static BsonDocument getUpdate(Guide guide)
    {
      string mongoQuery = "";
      if (guide.Title != null) mongoQuery += ", Title: '" + guide.Title + "'";
      if (guide.Description != null) mongoQuery += ", Description: '" + guide.Description + "'";
      if (guide.CreatorId != null) mongoQuery += ", CreatorId: '" + guide.CreatorId + "' ";
      if (guide.Sections != null) mongoQuery += ", Sections: '" + guide.Sections + "' ";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");

    }

    public static BsonDocument getUpdate(User user)
    {
      string mongoQuery = "";
      if (user.Username != null) mongoQuery += ", Username: '" + user.Username + "'";
      if (user.FirstName != null) mongoQuery += ", FirstName: '" + user.FirstName + "' ";
      if (user.LastName != null) mongoQuery += ", LastName: '" + user.LastName + "' ";
      if (user.Email != null) mongoQuery += ", Email: '" + user.Email + "' ";
      if (user.Introduction != null) mongoQuery += ", Introduction: '" + user.Introduction + "' ";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");
    }

    public static BsonDocument getUpdate(CodeLab codeLab)
    {
      string mongoQuery = "";
      if (codeLab.Title != null) mongoQuery += ", Title: '" + codeLab.Title + "'";
      if (codeLab.Description != null) mongoQuery += ", Description: '" + codeLab.Description + "' ";
      if (codeLab.LookingFor != null) mongoQuery += ", LookingFor: '" + codeLab.LookingFor + "' ";
      // if (codeLab.FinalGoalURL != null) mongoQuery += ", FinalGoalURL: '" + codeLab.FinalGoalURL + "' ";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");
    }

    public static BsonDocument getUpdate(Comment comment)
    {
      string mongoQuery = "";
      if (comment.Details != null) mongoQuery += ", Details: '" + comment.Details + "'";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");
    }

    private static BsonDocument createUpdate(string mongoQuery)
    {
      var doc = BsonDocument.Parse(mongoQuery);
      BsonDocument update = new BsonDocument("$set", doc);
      return update;
    }
  }
}