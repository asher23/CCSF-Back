using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class UserForReturnDto
  {
    public string Id { get; set; }
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Introduction { get; set; }

    public List<string> GuideIds { get; set; }

    public List<string> CommentIds { get; set; }

    public List<string> CodeLabIds { get; set; }
    public Photo Photo { get; set; }

  }
}