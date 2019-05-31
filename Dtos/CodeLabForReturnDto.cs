using System.Collections.Generic;
using FreshmanCSForum.API.Models;

namespace FreshmanCSForum.API.Dtos
{
  public class CodeLabForReturnDto
  {
    public string Id { get; set; }

    public string Title { get; set; }

    public string LeadId { get; set; }

    public string Description { get; set; }

    public string Goal { get; set; }

    public string LookingFor { get; set; }

    public string Joining { get; set; }

    public List<string> MemberIds { get; set; }

    // public List<string> CommentIds { get; set; }

    // public List<Photo> Photos { get; set; }

    public List<ApplicationForReturnDto> Applications { get; set; }
    public Chat Chat { get; set; }

  }
}