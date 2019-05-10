using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class GuideForUpdateAndRegisterDto
  {
    // [Required]
    public string Title { get; set; }

    public List<SubSectionForCreateDto> SubSections { get; set; }

    // [Required]
    public string Creator { get; set; }


  }
}