using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class UserForUpdateDto
  {
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public string Introduction { get; set; }

    public PhotoForCreationDto Photo { get; set; }

  }
}