using System;
using System.Collections.Generic;
using System.Linq;
using HighscoreApi.Models;

public class PlayerRepository {

  public IList<Player> GetPlayers () {
    using (var db = new HighscoreContext ()) {
      var players = db.Players.ToList ();
      Console.WriteLine ("Fetched {0} players", players.Count);
      return players;
    }
  }
}