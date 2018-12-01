using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighscoreApi.Dto;
using HighscoreApi.Entities;

namespace HighscoreApi.Repositories
{

  public class PlayersRepository : IPlayersRepository
  {

    private readonly HighscoreContext _dbContext;

    public PlayersRepository(HighscoreContext highscoreContext)
    {
      _dbContext = highscoreContext;
    }

    public Player GetSingle(int id)
    {
      return _dbContext.Players.FirstOrDefault(x => x.PlayerId == id);
    }

    public async Task<PlayerResponse> Add(PlayerCreate playerCreate)
    {
      var player = new Player { Name = playerCreate.Name };
      _dbContext.Players.Add(player);
      await _dbContext.SaveChangesAsync();
      await _dbContext.Entry(player).ReloadAsync();

      return new PlayerResponse { Name = player.Name, Id = player.PlayerId };
    }

    public void Delete(int id)
    {
      Player player = GetSingle(id);
      _dbContext.Players.Remove(player);
    }

    public Player Update(int id, Player item)
    {
      _dbContext.Players.Update(item);
      return item;
    }

    public int Count()
    {
      return _dbContext.Players.Count();
    }

    public bool Save()
    {
      return (_dbContext.SaveChanges() >= 0);
    }

    public IEnumerable<Player> GetAll()
    {
      return _dbContext.Players.AsEnumerable();
    }
  }
}