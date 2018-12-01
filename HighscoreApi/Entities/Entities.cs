using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Entities {

  public class Player {
    [Key]
    public int PlayerId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    public DateTime Created { get; set; }
    public ICollection<Result> Results { get; set; }
  }

  public class Score {
    [Key]
    public int ScoreId { get; set; }

    [Required]
    public int PlayerScore { get; set; }
    public int PlayerId { get; set; }
    public Player Player { get; set; }

  }

  public class Result {
    [Key]
    public int ResultId { get; set; }
    public ICollection<Score> Scores { get; set; }
  }
}