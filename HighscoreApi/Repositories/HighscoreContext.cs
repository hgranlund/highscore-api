using HighscoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Repositories {
  public class HighscoreContext : DbContext {
    public DbSet<Player> Players { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite ("Data Source=highscore.db");
    }
  }
}