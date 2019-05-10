namespace FreshmanCSForum.API.Dtos
{
  public class CommentForReturnDto
  {
    public string Id { get; set; }

    public string Details { get; set; }

    public string UserId { get; set; }

    public string Username { get; set; }

    public string PostId { get; set; }

    public string IsFor { get; set; }
  }
}