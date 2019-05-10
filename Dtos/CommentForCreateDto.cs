using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class CommentForCreateDto
  {
    [Required]
    public string Details { get; set; }

    public string PostId { get; set; }

    public string UserId { get; set; }

    public string Username { get; set; }

    public string IsFor { get; set; }

  }
}