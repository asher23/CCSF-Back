using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface IGuidesService
  {
    Task<IEnumerable<Guide>> GetAll();
    Task<IEnumerable<Guide>> GetAllForProfile(string creatorId);

    Task<GuideWithComments> GetOne(string id);
    Task Create(Guide guide);
    Task Delete(string id);
    Task<Guide> Update(string id, Guide guide);
    Task<GuideWithComments> AddComment(Comment comment);
    Task<string> GetCreatorId(string id);
  }
}