using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Models {
  public class HighscoreContext : DbContext {
    public DbSet<Player> Players { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite ("Data Source=highscore.db");
    }
  }

  public class Player {
    public int PlayerId { get; set; }
    public string Name { get; set; }

    public ICollection<Result> Results { get; set; }
  }

  public class Result {
    public int ResultId { get; set; }
    public int PlayerId { get; set; }
    public int PlayerIScore { get; set; }
    public Player Player { get; set; }
  }
}