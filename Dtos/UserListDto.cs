using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshmanCSForum.API.Dtos
{
  public class UserListDto
  {
    public List<string> userIds { get; set; }

  }
}