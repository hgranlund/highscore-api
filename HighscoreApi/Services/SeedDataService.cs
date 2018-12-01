using System;
using System.Threading.Tasks;
using HighscoreApi.Entities;
using HighscoreApi.Repositories;

namespace HighscoreApi.Services {
    public class SeedDataService : ISeedDataService {
        public async Task Initialize (HighscoreContext context) {
            context.Players.Add (new Player () { Name = "Caan Dalek" });
            await context.SaveChangesAsync ();
        }
    }
}