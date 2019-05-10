using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class UserForLoginDto
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "You need to specifiy a password before 4 and 8 characters")]
    public string Password { get; set; }
  }
}