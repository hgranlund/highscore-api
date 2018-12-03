using System.Collections.Generic;
using System.Threading.Tasks;
using HighscoreApi.Dto;
using HighscoreApi.Entities;

namespace HighscoreApi.Repositories
{
  public interface IPlayersRepository
  {
    Task<(PlayersRepositoryStatus Status, PlayerResponse Player)> GetSingle(int id);
    Task<PlayerResponse> Add(PlayerUpsert playerCreate);
    Task<(PlayersRepositoryStatus Status, PlayerResponse Player)> Update(int id, PlayerUpsert player);
    Task<PlayersRepositoryStatus> Delete(int id);
    int Count();
    bool Save();
    IEnumerable<Player> GetAll();
  }
}