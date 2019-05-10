using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface ICodeLabsService
  {
    Task<IEnumerable<CodeLab>> GetAll();
    Task<CodeLab> GetOne(string id);
    Task Create(CodeLab codeLab);
    Task Delete(string id);
    Task<CodeLab> Update(string id, CodeLab codeLab);
    Task<CodeLab> AddMember(string codeLabId, string userId);
  }
}