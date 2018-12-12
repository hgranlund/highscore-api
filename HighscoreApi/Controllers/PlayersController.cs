using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighscoreApi.Dto;
using HighscoreApi.Entities;
using HighscoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HighscoreApi.Controllers
{
  [Route("v1/[controller]")]
  [ApiController]
  public class PlayersController : ControllerBase
  {

    private readonly IPlayersRepository _repo;

    public PlayersController(IPlayersRepository playersRepository)
    {
      _repo = playersRepository;
    }

    [HttpGet(Name = nameof(GetAllPlayers))]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PlayerResponse>))]
    public async Task<ActionResult<IEnumerable<PlayerResponse>>> GetAllPlayers()
    {
      var allPlayers = await _repo.GetAll();
      return Ok(allPlayers);
    }

    [HttpGet("{id}", Name = nameof(GetSinglePlayer))]
    [ProducesResponseType(200, Type = typeof(PlayerResponse))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PlayerResponse>> GetSinglePlayer(int id)
    {
      var (status, player) = await _repo.GetSingle(id); ;
      switch (status)
      {
        case PlayersRepositoryStatus.NotFound: return NotFound(id);
        default: return Ok(player);
      }
    }

    [HttpPost(Name = nameof(AddPlayer))]
    [ProducesResponseType(201, Type = typeof(PlayerResponse))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PlayerResponse>> AddPlayer([FromBody] PlayerUpsert playerCreate)
    {
      var newPlayer = await _repo.Add(playerCreate);
      return CreatedAtRoute(nameof(GetSinglePlayer), new { id = newPlayer.Id }, newPlayer);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PlayerResponse))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]

    public async Task<ActionResult<PlayerResponse>> UpdatePlayer(int id, [FromBody] PlayerUpsert playerUpdate)
    {
      var (status, player) = await _repo.Update(id, playerUpdate);
      switch (status)
      {
        case PlayersRepositoryStatus.Updated: return Ok(player);
        default: return NotFound(id);
      }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(int))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<int>> DeletePlayer(int id)
    {
      var status = await _repo.Delete(id);

      switch (status)
      {
        case PlayersRepositoryStatus.Deleted:
          return Ok(id);
        default:
          return NotFound(id);
      }
    }
  }
}