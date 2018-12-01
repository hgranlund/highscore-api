using System.Threading.Tasks;
using HighscoreApi.Repositories;

namespace HighscoreApi.Services {
    public interface ISeedDataService {
        Task Initialize (HighscoreContext context);
    }
}