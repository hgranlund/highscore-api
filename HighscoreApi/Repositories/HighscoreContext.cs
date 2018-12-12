using HighscoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Repositories
{
  public class HighscoreContext : DbContext
  {
    public DbSet<Player> Players { get; set; }

    public HighscoreContext(DbContextOptions<HighscoreContext> options)
        : base(options)
    { }
  }
}