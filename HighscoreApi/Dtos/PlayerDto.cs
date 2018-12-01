using System.ComponentModel.DataAnnotations;

namespace HighscoreApi.Dto
{
  public class PlayerUpsert
  {
    [StringLength(200, MinimumLength = 2)]
    [Required]
    public string Name { get; set; }
  }

  public class PlayerResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}