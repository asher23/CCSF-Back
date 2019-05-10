using System.Collections.Generic;
using System.Threading.Tasks;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface IUsersService
  {
    Task<IEnumerable<User>> GetAll();
    Task<User> GetOne(string id);
    Task Delete(string id);
    Task<User> Update(string id, User user);
  }
}