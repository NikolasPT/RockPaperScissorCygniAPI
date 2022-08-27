using RockPaperScissorCygniAPI.Model;

namespace RockPaperScissorCygniAPI.DataRepository
{

    public class InMemGamesRepository : IGamesRepository
    {
        private readonly List<Game> games = new();

        public InMemGamesRepository(List<Game>? gamesIn = null)
        {
            if(gamesIn is not null)
                games.AddRange(gamesIn);

//#if DEBUG
//            games.Add(new Game(Guid.NewGuid(), new Player("Lisa", Move.Rock), new Player("Pelle", Move.Paper)));
//            games.Add(new Game(Guid.NewGuid(), new Player("Nikolas", Move.Rock), new Player("Peter", Move.Scissors)));
//#endif

        }


        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await Task.FromResult(games);
        }

        public async Task<Game?> GetGameAsync(Guid id)
        {
            var game = games.Where(game => game.Id == id).SingleOrDefault();
            if (game?.Id != Guid.Empty)
                return await Task.FromResult(game);

            return null;
        }

        public async Task SaveGameAsync(Game game)
        {
            if (game.Id == Guid.Empty)
                await Task.FromException(new ArgumentException(nameof(Game.Id)));

            games.Add(game);
            await Task.CompletedTask;
        }

        public async Task UpdateGameAsync(Game game)
        {
            var index = games.FindIndex(existingGame => existingGame.Id == game.Id);
            games[index] = game;
            await Task.CompletedTask;
        }


    }


}