using System.Collections.Generic;
using System.Threading.Tasks;
using HighscoreApi.Dto;
using HighscoreApi.Entities;

namespace HighscoreApi.Repositories {
  public interface IPlayersRepository {
    Player GetSingle(int id);
    Task<PlayerResponse> Add(PlayerCreate playerCreate);
    void Delete(int id);
    Player Update(int id, Player item);
    int Count();
    bool Save();
    IEnumerable<Player> GetAll();
  }
}