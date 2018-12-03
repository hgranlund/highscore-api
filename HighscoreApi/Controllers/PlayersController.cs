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
    public ActionResult<IEnumerable<PlayerResponse>> GetAllPlayers()
    {
      return Ok(_repo.GetAll());
    }

    [HttpGet("{id}", Name = "GetSinglePlayer")]
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
    public async Task<ActionResult<PlayerResponse>> AddPlayer([FromBody] PlayerUpsert playerCreate)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var newPlayer = await _repo.Add(playerCreate);
      return CreatedAtRoute("GetSinglePlayer", new { id = newPlayer.Id }, newPlayer);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(PlayerResponse))]
    [ProducesResponseType(404)]
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
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
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