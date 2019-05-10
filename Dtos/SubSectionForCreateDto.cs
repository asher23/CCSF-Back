using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class SubSectionForCreateDto
  {
    [Required]
    public string Title { get; set; }

    [Required]
    public string Details { get; set; }

    public string URLs { get; set; }

    public int RankNumber { get; set; }

    public List<SubSectionForCreateDto> InnerSubSections { get; set; }
  }
}