using System;
using Microsoft.AspNetCore.Http;

namespace FreshmanCSForum.API.Dtos
{
  public class PhotoForCreationDto
  {
    public string Url { get; set; }
    public string File { get; set; }
    public DateTime DateAdded { get; set; }
    public string PublicId { get; set; }
    public PhotoForCreationDto()
    {
      DateAdded = DateTime.Now;
    }
  }
}