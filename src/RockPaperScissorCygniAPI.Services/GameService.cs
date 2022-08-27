using Microsoft.AspNetCore.Mvc;
using RockPaperScissorCygniAPI.DataRepository;
using RockPaperScissorCygniAPI.Model;
using RockPaperScissorCygniAPI.Model.Dtos;

namespace RockPaperScissorCygniAPI.Services
{

    public class GameService : IGameService
    {
        private readonly IGamesRepository repository;


        public GameService(IGamesRepository repository)
        {
            this.repository = repository;
        }


        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync()
        {
            var games = (await repository.GetGamesAsync()).Select(game => game.AsDto());
            return  new OkObjectResult(games);
        }


        public async Task<ActionResult<GameDto>> GetGameAsync(Guid id)
        {
            var game = await repository.GetGameAsync(id);

            if (game is null)
                return new NotFoundObjectResult("No game found with id = " + id.ToString());

            // If the game is unfinished we hide the moves to prevent cheating
            if (game.GameOutcome == Outcome.Unfinished)
            {
                Game gameRestricted = new(game.Id,
                    new Player(game.Player1.Name),
                    new Player(game.Player2.Name));
                return new OkObjectResult(gameRestricted.AsDto());
            }
            else
                return new OkObjectResult(game.AsDto());
        }


        public async Task<Game> CreateGameAsync(CreateGameDto createGameDto)
        {
            Game game = new(Guid.NewGuid(), new Player(createGameDto.Name), new Player());

            await repository.SaveGameAsync(game);

            return game;
        }


        public async Task<ActionResult> JoinGameAsync(Guid id, JoinGameDto joinGameDto)
        {
            Game? existingGame = await repository.GetGameAsync(id);

            if (existingGame is null)
                return new NotFoundObjectResult("No game found with id = " + id.ToString());

            if (existingGame.Player1 is null)
                return new BadRequestObjectResult("Player1 is null in game with id = " + id.ToString());

            if (!string.IsNullOrEmpty(existingGame.Player1?.Name) &&
                !string.IsNullOrEmpty(existingGame.Player2?.Name))
            {
                return new BadRequestObjectResult($"Game with id: {id} is already full.");
            }

            if (existingGame.Player1?.Name == joinGameDto.Name ||
                existingGame.Player2?.Name == joinGameDto.Name)
            {
                return new BadRequestObjectResult($"A player with name {joinGameDto.Name} has already joined the game.");
            }

            Game updatedGame = new(existingGame.Id)
            {
                Player1 = existingGame?.Player1 ?? throw new NullReferenceException("existingGame?.Player1 is null."),
                Player2 = new Player(joinGameDto.Name)
            };

            await repository.UpdateGameAsync(updatedGame);
            return new NoContentResult();
        }


        public async Task<ActionResult> MoveAsync(Guid id, MoveDto moveDto)
        {
            Game? existingGame = await repository.GetGameAsync(id);

            if (existingGame is null)
                return new NotFoundObjectResult("No game found with id = " + id.ToString());

            if (existingGame.Player1 is null)
                return new BadRequestObjectResult("Player1 is null in game with id = " + id.ToString());

            if (existingGame.Player2 is null)
                return new BadRequestObjectResult("Player2 is null in game with id = " + id.ToString());

            if (existingGame.Player1?.Name != moveDto.Name && existingGame.Player2?.Name != moveDto.Name)
                return new BadRequestObjectResult($"A player with name {moveDto.Name} is not connected to the game.");

            if (existingGame.Player1?.Name == moveDto.Name && existingGame.Player1?.Move != Move.NA)
                return new BadRequestObjectResult("Player1 has already made a move.");                
            
            if (existingGame.Player2?.Name == moveDto.Name && existingGame.Player2?.Move != Move.NA)
                return new BadRequestObjectResult("Player2 has already made a move.");

            string player1Name = existingGame?.Player1?.Name ?? throw new NullReferenceException("existingGame?.Player1?.Name is null.");
            string player2Name = existingGame?.Player2?.Name ?? throw new NullReferenceException("existingGame?.Player2?.Name is null.");

            Game updatedGame = new(existingGame.Id)
            {
                Player1 = player1Name == moveDto.Name ?
                    moveDto.MoveDtoAsPlayer() :
                    existingGame.Player1,
                Player2 = player2Name == moveDto.Name ?
                    moveDto.MoveDtoAsPlayer() :
                    existingGame.Player2
            };

            await repository.UpdateGameAsync(updatedGame);
            return new NoContentResult();
        }


    }
}
