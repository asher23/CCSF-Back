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
      if (guide.UserId != null) mongoQuery += ", Creator: '" + guide.UserId + "' ";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");

    }

    public static BsonDocument getUpdate(User user)
    {
      string mongoQuery = "";
      if (user.UserName != null) mongoQuery += ", Username: '" + user.UserName + "'";
      if (user.FirstName != null) mongoQuery += ", FirstName: '" + user.FirstName + "' ";
      if (user.LastName != null) mongoQuery += ", LastName: '" + user.LastName + "' ";
      if (user.Email != null) mongoQuery += ", Email: '" + user.Email + "' ";
      return createUpdate("{" + mongoQuery.Substring(1) + "}");
    }

    public static BsonDocument getUpdate(CodeLab codeLab)
    {
      string mongoQuery = "";
      if (codeLab.Title != null) mongoQuery += ", Title: '" + codeLab.Title + "'";
      if (codeLab.Description != null) mongoQuery += ", Description: '" + codeLab.Description + "' ";
      if (codeLab.LookingFor != null) mongoQuery += ", LookingFor: '" + codeLab.LookingFor + "' ";
      if (codeLab.FinalGoalURL != null) mongoQuery += ", FinalGoalURL: '" + codeLab.FinalGoalURL + "' ";
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