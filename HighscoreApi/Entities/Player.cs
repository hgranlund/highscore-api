using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Entities
{

  public class Player
  {
    [Key]
    public int PlayerId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    public DateTime Created { get; set; }
  }
}