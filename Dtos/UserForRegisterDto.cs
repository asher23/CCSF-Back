using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class UserForRegisterDto
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "You need to specifiy a password before 4 and 8 characters")]
    public string Password { get; set; }

    // [Required]
    public string FirstName { get; set; }

    // [Required]
    public string LastName { get; set; }

    // [Required]
    // [EmailAddress]
    public string Email { get; set; }

  }
}