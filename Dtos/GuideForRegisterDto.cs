using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class GuideForRegisterDto
  {
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    // [Required]
    public string CreatorId { get; set; }
  }
}