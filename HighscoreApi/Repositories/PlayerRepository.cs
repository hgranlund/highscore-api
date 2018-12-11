using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighscoreApi.Dto;
using HighscoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HighscoreApi.Repositories
{
  public enum PlayersRepositoryStatus
  {
    Updated,
    Found,
    NotFound,
    Deleted
  }

  public class PlayersRepository : IPlayersRepository
  {

    private readonly HighscoreContext _dbContext;

    public PlayersRepository(HighscoreContext highscoreContext)
    {
      _dbContext = highscoreContext;
    }

    public async Task<(PlayersRepositoryStatus Status, PlayerResponse Player)> GetSingle(int id)
    {
      var player = await _dbContext.Players.FindAsync(id);
      if (player == null)
      {
        return (PlayersRepositoryStatus.NotFound, null);
      }
      var playerResponse = new PlayerResponse
      {
        Name = player.Name,
        Id = player.PlayerId
      };
      return (PlayersRepositoryStatus.Found, playerResponse);
    }

    public async Task<PlayerResponse> Add(PlayerUpsert playerCreate)
    {
      var player = new Player { Name = playerCreate.Name, Created = DateTime.Now };
      _dbContext.Players.Add(player);
      await _dbContext.SaveChangesAsync();
      await _dbContext.Entry(player).ReloadAsync();

      return new PlayerResponse
      {
        Name = player.Name,
        Id = player.PlayerId
      };
    }
    public async Task<(PlayersRepositoryStatus Status, PlayerResponse Player)> Update(int id, PlayerUpsert player)
    {
      var playerToUpdate = await _dbContext.Players.FindAsync(id);
      if (playerToUpdate == null)
      {
        return (PlayersRepositoryStatus.NotFound, null);
      }

      playerToUpdate.Name = player.Name;
      await _dbContext.SaveChangesAsync();

      return (PlayersRepositoryStatus.Updated, MapPlayerToPlayerResponse(playerToUpdate));
    }

    public async Task<PlayersRepositoryStatus> Delete(int id)
    {
      var player = await _dbContext.Players.FindAsync(id);
      if (player == null)
      {
        return PlayersRepositoryStatus.NotFound;
      }
      _dbContext.Players.Remove(player);
      await _dbContext.SaveChangesAsync();
      return PlayersRepositoryStatus.Deleted;
    }

    public int Count()
    {
      return _dbContext.Players.Count();
    }

    public bool Save()
    {
      return (_dbContext.SaveChanges() >= 0);
    }

    public async Task<IEnumerable<PlayerResponse>> GetAll()
    {
      var players = await _dbContext.Players.AsQueryable().ToListAsync();
      return players.Select(MapPlayerToPlayerResponse);
    }

    private PlayerResponse MapPlayerToPlayerResponse(Player player)
    {
      return new PlayerResponse
      {
        Id = player.PlayerId,
        Name = player.Name
      };
    }
  }
}