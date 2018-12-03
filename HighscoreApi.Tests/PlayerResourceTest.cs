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

namespace HighscoreApi.Tests
{
  public class PlayerResourceTest
  {
    private readonly PlayersController _controller;

    public PlayerResourceTest()
    {
      var services = new ServiceCollection()
        .AddScoped<IPlayersRepository, PlayersRepository>()
        .AddDbContext<HighscoreContext>(options => options.UseInMemoryDatabase(databaseName: "highscore_in_memory_test"))
        .BuildServiceProvider();

      _controller = new PlayersController(services.GetService<IPlayersRepository>());
    }

    protected PlayerResponse GetPlayerFromResponse(ActionResult<PlayerResponse> response)
    {
      if (response.Result is CreatedAtRouteResult)
      {
        var created = (CreatedAtRouteResult)response.Result;
        return (PlayerResponse)created.Value;
      }
      else if (response.Result is ObjectResult)
      {
        var created = (ObjectResult)response.Result;
        return (PlayerResponse)created.Value;
      }
      else
      {
        throw new Exception("Unable to get PlayerResponse");
      }
    }

    public class WhenCreatingPlayer : PlayerResourceTest
    {
      [Fact]
      public async Task WithALegalInput_APlayerShouldBeCreated()
      {
        var newPlayer = new PlayerUpsert { Name = "Davros Dalek" };
        var response = await _controller.AddPlayer(newPlayer);

        response.Result.Should().BeOfType<CreatedAtRouteResult>();

        PlayerResponse player = GetPlayerFromResponse(response);
        player.Name.Should().Be(newPlayer.Name);
        player.Id.Should().NotBe(0);
      }
    }
    public class WhenGettingSinglePlayer : PlayerResourceTest, IAsyncLifetime
    {
      private PlayerResponse existingPlayer;
      public async Task InitializeAsync()
      {
        var newPlayer = new PlayerUpsert { Name = "Existing Dalek" };
        var result = await _controller.AddPlayer(newPlayer);
        existingPlayer = GetPlayerFromResponse(result);
      }

      public Task DisposeAsync()
      {
        return Task.CompletedTask;
      }

      [Fact]
      public async Task AndPlayerExists_ShouldReturnThePlayer()
      {
        var response = await _controller.GetSinglePlayer(existingPlayer.Id);
        GetPlayerFromResponse(response).Name.Should().Be(existingPlayer.Name);
      }

      [Fact]
      public async Task AndPlayerDontExixts_ShouldReturnNotFound()
      {
        var response = await _controller.GetSinglePlayer(123456);
        response.Result.Should().BeOfType<NotFoundObjectResult>();
      }
    }
  }
}
