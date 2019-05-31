
namespace FreshmanCSForum.API.Dtos
{
  public class ApplicationForReturnDto
  {
    public string Id { get; set; }

    public UserForReturnDto Applicant { get; set; }

    public string Message { get; set; }

    public string Status { get; set; }
  }
}