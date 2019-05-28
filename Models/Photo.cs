using System;

namespace FreshmanCSForum.API.Models
{
  public class Photo
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public DateTime DateAdded { get; set; }
    public string PublicId { get; set; }
  }
}