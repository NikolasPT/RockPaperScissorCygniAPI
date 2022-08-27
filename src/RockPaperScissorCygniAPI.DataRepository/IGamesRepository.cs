using RockPaperScissorCygniAPI.Model;

namespace RockPaperScissorCygniAPI.DataRepository
{
    public interface IGamesRepository
    {
        public Task<IEnumerable<Game>> GetGamesAsync();
        public Task<Game?> GetGameAsync(Guid id);
        public Task SaveGameAsync(Game game);
        public Task UpdateGameAsync(Game game);
    }


}