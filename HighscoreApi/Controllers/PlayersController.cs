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
        public ActionResult<PlayerResponse> GetSinglePlayer(int id)
        {
            Player player = _repo.GetSingle(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost(Name = nameof(AddPlayer))]
        public async Task<ActionResult<Player>> AddPlayer([FromBody] PlayerCreate playerCreate)
        {
            var newPlayer = await _repo.Add(playerCreate);
            return CreatedAtRoute("GetSinglePlayer", new { id = newPlayer.Id },
                newPlayer);
        }

        [HttpPut("{id:int}", Name = nameof(UpdatePlayer))]
        public void UpdatePlayer(int id, [FromBody] string value) {}

        [HttpDelete("{id}")]
        public void DeletePlayer(int id) {}
    }
}