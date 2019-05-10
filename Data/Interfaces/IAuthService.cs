using System.Threading.Tasks;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Data.Interfaces
{
  public interface IAuthService
  {
    Task<User> Login(string username, string password);
    Task<User> Register(User user, string password);
    Task<bool> UserExists(string username);

  }
}