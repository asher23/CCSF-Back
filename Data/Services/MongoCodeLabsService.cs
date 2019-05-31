using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Helpers;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Data
{
  public class MongoCodeLabsService : ICodeLabsService
  {
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<CodeLab> _codeLabs;
    private readonly IMapper _mapper;
    public MongoCodeLabsService(MyMongoDatabase database, IMapper mapper)
    {
      _codeLabs = database.CodeLabs;
      _users = database.Users;
      _mapper = mapper;
    }

    public async Task<CodeLabForReturnDto> AddApplicant(string codeLabId, Application application)
    {
      // FieldDefinition<CodeLab> field = "Applications";
      application.Status = "waiting";
      var updateResult = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId),
        Builders<CodeLab>.Update.Push(x => x.Applications, application),
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }

      );
      return await GetOne(codeLabId);
    }

    public async Task<CodeLabForReturnDto> AddMember(string codeLabId, string userId)
    {
      var updateResult = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId),
        Builders<CodeLab>.Update.Push(x => x.MemberIds, codeLabId),
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
      );
      return await GetOne(codeLabId);
    }

    public async Task<CodeLabForReturnDto> AcceptApplication(string codeLabId, string applicantId, string userId)
    {
      CodeLab codeLab = await _codeLabs.Find(Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId)).FirstOrDefaultAsync();
      if (!codeLab.MemberIds.Contains(userId))
      {
        throw new UnauthorizedAccessException();
      }
      // Application application = codeLab.Applications.Find(app => app.Id == applicantId);

      var updateResult = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId),
        Builders<CodeLab>.Update.Push(x => x.MemberIds, applicantId),
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
      );

      var filter = Builders<CodeLab>.Filter;
      var fullFilter = filter.And(
        filter.Eq(x => x.Id, codeLabId),
        filter.ElemMatch(x => x.Applications, a => a.ApplicantId == applicantId));
      var update = Builders<CodeLab>.Update;
      var applicationStatusSetter = update.Set("Applications.$.Status", "accepted");
      await _codeLabs.UpdateOneAsync(fullFilter, applicationStatusSetter);
      return await GetOne(codeLabId);
    }

    public async Task<CodeLabForReturnDto> RejectApplication(string codeLabId, string applicantId, string userId)
    {
      CodeLab codeLab = await _codeLabs.Find(Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId)).FirstOrDefaultAsync();
      if (!codeLab.MemberIds.Contains(userId))
      {
        throw new UnauthorizedAccessException();
      }

      var filter = Builders<CodeLab>.Filter;
      var fullFilter = filter.And(
        filter.Eq(x => x.Id, codeLabId),
        filter.ElemMatch(x => x.Applications, a => a.ApplicantId == applicantId));
      var update = Builders<CodeLab>.Update;
      var applicationStatusSetter = update.Set("Applications.$.Status", "rejected");
      await _codeLabs.UpdateOneAsync(fullFilter, applicationStatusSetter);
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

    public async Task<CodeLabForReturnDto> GetOne(string id)
    {

      CodeLab codeLab = await _codeLabs.Find(Builders<CodeLab>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
      CodeLabForReturnDto codeLabToReturnDto = _mapper.Map<CodeLabForReturnDto>(codeLab);

      var applications = new List<ApplicationForReturnDto>();
      foreach (Application application in codeLab.Applications)
      {
        User user = await _users.Find(Builders<User>.Filter.Eq(x => x.Id, application.ApplicantId)).FirstOrDefaultAsync();
        UserForReturnDto userForReturn = _mapper.Map<UserForReturnDto>(user);
        ApplicationForReturnDto applicationForReturnDto = _mapper.Map<ApplicationForReturnDto>(application);
        applicationForReturnDto.Applicant = userForReturn;
        applications.Add(applicationForReturnDto);
      }
      codeLabToReturnDto.Applications = applications;
      return codeLabToReturnDto;
    }

    public async Task<CodeLab> Update(string id, CodeLab codeLab)
    {
      var updateList = new List<UpdateDefinition<CodeLab>>();
      if (codeLab.Title != null) updateList.Add(Builders<CodeLab>.Update.Set("Title", codeLab.Title));
      if (codeLab.Description != null) updateList.Add(Builders<CodeLab>.Update.Set("Description", codeLab.Description));
      if (codeLab.Goal != null) updateList.Add(Builders<CodeLab>.Update.Set("Goal", codeLab.Goal));
      if (codeLab.LookingFor != null) updateList.Add(Builders<CodeLab>.Update.Set("LookingFor", codeLab.LookingFor));
      if (codeLab.Joining != null) updateList.Add(Builders<CodeLab>.Update.Set("Joining", codeLab.Joining));
      var finalUpdate = Builders<CodeLab>.Update.Combine(updateList);

      CodeLab result = await _codeLabs.FindOneAndUpdateAsync(
        Builders<CodeLab>.Filter.Eq(x => x.Id, id),
        finalUpdate,
        new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
      );
      return result;
    }

    public async Task<CodeLab> AddMessageToChat(string codeLabId, ChatMessage chatMessage)
    {
      //       CodeLab codeLab = await _codeLabs.Find(Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId)).FirstOrDefaultAsync();
      // if (!codeLab.MemberIds.Contains(userId))
      // {
      //   throw new UnauthorizedAccessException();
      // }

      var updateResult = await _codeLabs.FindOneAndUpdateAsync(
       Builders<CodeLab>.Filter.Eq(x => x.Id, codeLabId),
       Builders<CodeLab>.Update.Push(x => x.Chat.Messages, chatMessage),
       new FindOneAndUpdateOptions<CodeLab> { ReturnDocument = ReturnDocument.After }
     );

      return updateResult;
    }
  }
}