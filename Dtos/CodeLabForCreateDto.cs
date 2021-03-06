using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class CodeLabForCreateDto
  {
    public string Title { get; set; }

    public string Description { get; set; }
    public string Goal { get; set; }


    // [Required]
    public string LookingFor { get; set; }
    public string Joining { get; set; }

    public Chat Chat { get; set; }


  }
}