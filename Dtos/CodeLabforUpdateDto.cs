using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class CodeLabForUpdateDto
  {
    public string Title { get; set; }

    public string Description { get; set; }
    public string Goal { get; set; }

    public string LookingFor { get; set; }
    public string Joining { get; set; }

    public Chat Chat { get; set; }

  }
}