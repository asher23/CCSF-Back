using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Dtos;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface ICodeLabsService
  {
    Task<IEnumerable<CodeLab>> GetAll();
    Task<CodeLabForReturnDto> GetOne(string id);
    Task Create(CodeLab codeLab);
    Task Delete(string id);
    Task<CodeLab> Update(string id, CodeLab codeLab);
    Task<CodeLabForReturnDto> AddMember(string codeLabId, string userId);
    Task<CodeLabForReturnDto> AddApplicant(string codeLabId, Application application);

    Task<CodeLabForReturnDto> AcceptApplication(string codeLabId, string applicantId, string userId);
    Task<CodeLabForReturnDto> RejectApplication(string codeLabId, string applicantId, string userId);

    Task<CodeLab> AddMessageToChat(string codeLabId, ChatMessage chatMessage);

  }
}