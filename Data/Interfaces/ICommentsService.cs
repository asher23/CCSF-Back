using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface ICommentsService
  {
    Task<IEnumerable<Comment>> GetAllForUser(string userId);
    Task<IEnumerable<Comment>> GetAllForPost(string postId);
    Task Create(Comment comment);
    Task<Comment> GetOne(string id);
    Task Delete(string id);
    Task<Comment> Update(string id, Comment comment);

  }
}