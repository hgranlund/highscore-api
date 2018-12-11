using System;
using Xunit;
using HighscoreApi.Controllers;
using Microsoft.EntityFrameworkCore;
using HighscoreApi.Repositories;
using HighscoreApi.Dto;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HighscoreApi.Tests
{
  public class PlayerResource
  {
    private readonly PlayersController _controller;

    public PlayerResource()
    {
      var services = new ServiceCollection()
        .AddScoped<IPlayersRepository, PlayersRepository>()
        .AddDbContext<HighscoreContext>(options => options.UseInMemoryDatabase(databaseName: "highscore_in_memory_test"))
        .BuildServiceProvider();

      _controller = new PlayersController(services.GetService<IPlayersRepository>());
    }
    public async Task<PlayerResponse> CreatePlayer(string name)
    {
      var newPlayer = new PlayerUpsert { Name = name };
      var response = await _controller.AddPlayer(newPlayer);
      return response.Get<PlayerResponse>();
    }

    public class WhenAddingPlayer : PlayerResource
    {
      [Fact]
      public async Task WithALegalInput_APlayerShouldBeCreated()
      {
        var newPlayer = new PlayerUpsert { Name = "Davros Dalek" };
        var response = await _controller.AddPlayer(newPlayer);

        response.Result.Should().BeOfType<CreatedAtRouteResult>();
        response.Get<PlayerResponse>().Name.Should().Be(newPlayer.Name);
      }
    }
    public class WhenGettingSinglePlayer : PlayerResource
    {

      [Fact]
      public async Task AndPlayerExists_ShouldReturnThePlayer()
      {
        var player = await CreatePlayer("Davros Dalek");
        var response = await _controller.GetSinglePlayer(player.Id);
        response.Get<PlayerResponse>().Name.Should().Be(player.Name);
      }

      [Fact]
      public async Task AndPlayerDontExixts_ShouldReturnNotFound()
      {
        var response = await _controller.GetSinglePlayer(123456);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
    }
    public class WhenDeletingPlayer : PlayerResource
    {
      [Fact]
      public async Task AndPlayerExists_ShouldDeleteThePlayer()
      {
        var player = await CreatePlayer("Davros Dalek");
        await _controller.DeletePlayer(player.Id);
        var response = await _controller.GetSinglePlayer(player.Id);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }

      [Fact]
      public async Task AndPlayerDontExixts_ShouldReturnNotFound()
      {
        var response = await _controller.DeletePlayer(123456);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
    }
    public class WhenGettingAllPlayers : PlayerResource
    {
      [Fact]
      public async Task ShouldReturnAllPlayers()
      {
        var playersToCreate = new List<string>() { "Davros Dalek", "Doctor Who" };
        var createdPlayers = playersToCreate.Select(async name => await CreatePlayer(name))
                              .Select(task => task.Result).ToList();

        var response = await _controller.GetAllPlayers();
        var players = response.Get<IEnumerable<PlayerResponse>>().ToList();
        players.Should().BeEquivalentTo(createdPlayers);
      }

      [Fact]
      public async Task AndPlayerDontExixts_ShouldReturnNotFound()
      {
        var response = await _controller.DeletePlayer(123456);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
    }
  }
}
