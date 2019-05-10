using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Helpers;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Data
{
  public class MongoCodeLabsService : ICodeLabsService
  {
    private readonly IMongoCollection<CodeLab> _codeLabs;
    public MongoCodeLabsService(MyMongoDatabase database)
    {
      _codeLabs = database.CodeLabs;
    }
    public async Task<CodeLab> AddMember(string codeLabId, string userId)
    {
      var updateResult = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId),
        Builders<CodeLab>.Update.Push(x => x.MemberIds, codeLabId),
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
      );
      return await GetOne(codeLabId);
    }

    public Task Create(CodeLab codeLab)
    {
      return _codeLabs.InsertOneAsync(codeLab);
    }

    public Task Delete(string id)
    {
      return _codeLabs.DeleteOneAsync(Builders<CodeLab>.Filter.Eq(x => x.Id, id));
    }

    public async Task<IEnumerable<CodeLab>> GetAll()
    {
      return await _codeLabs.Find(Builders<CodeLab>.Filter.Empty).ToListAsync();
    }

    public Task<CodeLab> GetOne(string id)
    {
      return _codeLabs.Find(Builders<CodeLab>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
    }

    public async Task<CodeLab> Update(string id, CodeLab codeLabChanges)
    {
      CodeLab result = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, id),
        UpdateBuilders.getUpdate(codeLabChanges),
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
      );
      return result;
    }
  }
}