using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class GuideForUpdateAndRegisterDto
  {
    // [Required]
    public string Title { get; set; }

    // [Required]
    public string Description { get; set; }
    public List<SectionForCreateDto> Sections { get; set; }

    // [Required]
    public string Creator { get; set; }

    public List<PhotoForCreationDto> Photos { get; set; }
  }
}