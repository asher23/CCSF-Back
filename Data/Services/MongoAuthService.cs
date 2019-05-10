using System.Threading.Tasks;
using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Models;
using MongoDB.Driver;

namespace FreshmanCSForum.API.Data
{
  public class MongoAuthService : IAuthService
  {

    private readonly IMongoCollection<User> _users;
    public MongoAuthService(MyMongoDatabase database)
    {
      _users = database.Users;
    }
    public async Task<User> Login(string username, string password)
    {
      var user = await _users.Find<User>(Builders<User>.Filter.Eq(x => x.UserName, username)).FirstOrDefaultAsync();
      if (user == null) return null;
      if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
      return user;
    }

    public async Task<User> Register(User user, string password)
    {
      byte[] passwordSalt, passwordHash;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);
      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;
      await _users.InsertOneAsync(user);
      return user;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != passwordHash[i]) return false;
        }
      }
      return true;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    public async Task<bool> UserExists(string username)
    {
      if (await _users.CountDocumentsAsync(Builders<User>.Filter.Eq(x => x.UserName, username)) > 0)
        return true;

      return false;
    }
  }
}