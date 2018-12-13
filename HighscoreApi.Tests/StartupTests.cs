using System;
using Xunit;
using HighscoreApi.Controllers;
using HighscoreApi.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace HighscoreApi.Tests
{
  public class StartupTests
  {
    [Fact]
    public void ShouldBeAbleToInstantiatePlayersController()
    {
      var services = new ServiceCollection();
      new Startup(null).ConfigureServices(services);
      var provider = services.BuildServiceProvider();
      var repo = provider.GetService<IPlayersRepository>();
      var controller = new PlayersController(repo);
      controller.Should().NotBeNull();
    }
  }
}