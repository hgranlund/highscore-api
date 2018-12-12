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
  public class GivenAPlayerController
  {
    private readonly PlayersController _controller;

    public GivenAPlayerController()
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

    public class WhenNoPlayersHasBeenAdded : GivenAPlayerController
    {

      [Fact]
      public async Task ShouldBeAbleToAddValidPlayer()
      {
        var newPlayer = new PlayerUpsert { Name = "Davros Dalek" };
        var response = await _controller.AddPlayer(newPlayer);
        Console.WriteLine("En Gang");
        response.Result.Should().BeOfType<CreatedAtRouteResult>();
        response.Get<PlayerResponse>().Name.Should().Be(newPlayer.Name);
      }

      [Fact]
      public async void ShouldNotBeAnyPlayersInStore()
      {
        var response = await _controller.GetAllPlayers();
        response.Get<IEnumerable<PlayerResponse>>().Should().BeEmpty();
      }
    }

    public class WhenAPlayerHasBeenAdded : GivenAPlayerController, IAsyncLifetime
    {
      public PlayerResponse existingPlayer;
      public async Task InitializeAsync()
      {
        existingPlayer = await CreatePlayer("Davros Dalek");
      }

      public Task DisposeAsync()
      {
        return Task.CompletedTask;
      }

      [Fact]
      public async Task ShouldBeAbleToGetPlayer()
      {
        var response = await _controller.GetSinglePlayer(existingPlayer.Id);
        response.Get<PlayerResponse>().Should().BeEquivalentTo(existingPlayer);
      }

      [Fact]
      public async void ShouldBeAbleToDeletePlayer()
      {
        await _controller.DeletePlayer(existingPlayer.Id);

        var response = await _controller.GetSinglePlayer(existingPlayer.Id);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
      [Fact]
      public async void ShouldNotBeAbleToGetANonExistingPlayer()
      {
        var response = await _controller.GetSinglePlayer(1233);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
    }

    public class WhenSomePlayersHasBeenAdded : GivenAPlayerController, IAsyncLifetime
    {
      public List<PlayerResponse> existingPlayers;
      public async Task InitializeAsync()
      {
        var dalek = await CreatePlayer("Davros Dalek");
        var docktor = await CreatePlayer("Doctor Who");
        existingPlayers = new List<PlayerResponse>() { dalek, docktor };
      }

      public Task DisposeAsync()
      {
        return Task.CompletedTask;
      }

      [Fact]
      public async Task ShouldBeAbleToGetAllPlayers()
      {
        var response = await _controller.GetAllPlayers();
        var players = response.Get<IEnumerable<PlayerResponse>>().ToList();
        players.Should().BeEquivalentTo(existingPlayers);
      }

      [Fact]
      public async Task ShouldBeAbleToGetPlayer()
      {
        var existingPlayer = existingPlayers.Last();
        var response = await _controller.GetSinglePlayer(existingPlayer.Id);
        response.Get<PlayerResponse>().Name.Should().Be(existingPlayer.Name);
      }
    }
  }
}
