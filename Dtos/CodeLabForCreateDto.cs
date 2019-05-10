namespace FreshmanCSForum.API.Dtos
{
  public class CodeLabForCreateDto
  {
    public string Title { get; set; }

    public string Description { get; set; }

    // [Required]
    public string LookingFor { get; set; }

    public string FinalGoalURL { get; set; }

  }
}