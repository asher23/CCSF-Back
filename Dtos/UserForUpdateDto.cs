using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class UserForUpdateDto
  {
    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
  }
}